using UnityEngine;
using Unity.Barracuda;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;
using TMPro;
using Unity.Collections; // For NativeArray and Allocator


public class Emotion : MonoBehaviour
{
    public NNModel emotionModelAsset;  // Assign your ONNX model here in the Inspector
    private Model emotionModel;
    private IWorker worker;
    private ARCameraManager cameraManager;

    public TextMeshProUGUI emotionText;  // TextMeshPro text field for showing the emotion

    void Start()
    {
        // Load the pre-trained model
        emotionModel = ModelLoader.Load(emotionModelAsset);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, emotionModel);

        // Get AR Camera Manager for the camera feed
        cameraManager = FindObjectOfType<ARCameraManager>();
    }

    void OnDestroy()
    {
        // Dispose of the worker when done
        worker.Dispose();
    }

    void Update()
    {
        // Check if we can get the latest camera image
        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            // Start a coroutine to process the image asynchronously
            StartCoroutine(ProcessImage(image));
        }
    }

    IEnumerator ProcessImage(XRCpuImage image)
    {
        // Create conversion parameters
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(64, 64),  // Adjust as per your model input size
            outputFormat = TextureFormat.R8,  // Grayscale (or change based on your model)
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // Allocate a buffer to store the image
        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Convert the image
        image.Convert(conversionParams, buffer);

        // Dispose the image as we don't need it anymore
        image.Dispose();

        // Create a Tensor from the image data
        int width = conversionParams.outputDimensions.x;
        int height = conversionParams.outputDimensions.y;
        Tensor input = new Tensor(1, height, width, 1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int pixelIndex = y * width + x;
                float pixelValue = buffer[pixelIndex] / 255f;  // Normalize pixel value
                input[0, y, x, 0] = pixelValue;
            }
        }

        buffer.Dispose();

        // Run the model with the input tensor
        worker.Execute(input);

        // Get the output tensor
        Tensor output = worker.PeekOutput();

        // Interpret the model output and update the UI
        InterpretModelOutput(output);

        // Dispose tensors
        input.Dispose();
        output.Dispose();

        yield return null;
    }

    private void InterpretModelOutput(Tensor output)
    {
        // Get the index of the highest probability (most likely emotion)
        int predictedEmotion = output.ArgMax()[0];

        // Map the index to an emotion label
        string emotion = MapEmotion(predictedEmotion);

        // Log the detected emotion
        Debug.Log("Detected Emotion: " + emotion);

        // Update the TMP text element if available
        if (emotionText != null)
        {
            emotionText.text = "Emotion: " + emotion;
            Debug.Log("Emotion text updated.");
        }
        else
        {
            Debug.LogWarning("Emotion Text is not assigned.");
        }
    }

    private string MapEmotion(int predictedEmotion)
    {
        // Adjust this mapping based on your model's output labels
        string[] emotions = { "Neutral", "Happiness", "Surprise", "Sadness", "Anger", "Disgust", "Fear", "Contempt" };
        if (predictedEmotion >= 0 && predictedEmotion < emotions.Length)
        {
            return emotions[predictedEmotion];
        }
        else
        {
            Debug.LogWarning("Predicted emotion index out of range: " + predictedEmotion);
            return "Unknown";
        }
    }
}
