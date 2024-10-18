using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;  // Add this line
using Unity.Collections.LowLevel.Unsafe; // Add if using GetUnsafePtr()
using TMPro;





public class Emotion : MonoBehaviour
{
    public NNModel emotionModelAsset;
    private Model emotionModel;
    private IWorker worker;
    private ARCameraManager cameraManager;
    public TextMeshProUGUI emotionText; // Add this line


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

    private float timeSinceLastProcess = 0f;
    public float processingInterval = 0.5f; // Process every 0.5 seconds

    void Update()
    {
        timeSinceLastProcess += Time.deltaTime;
        if (timeSinceLastProcess >= processingInterval)
        {
            if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                StartCoroutine(ProcessImage(image));
                timeSinceLastProcess = 0f;
            }
        }
    }


    IEnumerator ProcessImage(XRCpuImage image)
    {
        // Create conversion parameters
        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the full image
            inputRect = new RectInt(0, 0, image.width, image.height),
            // Downsample to match model's input size (e.g., 64x64)
            outputDimensions = new Vector2Int(64, 64),
            // Choose format compatible with the model (e.g., Grayscale)
            outputFormat = TextureFormat.R8,
            // Mirror image vertically if needed
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // Allocate a buffer to store the image
        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Convert the image directly into the buffer without using unsafe code
        image.Convert(conversionParams, buffer);

        // Dispose the image as we don't need it anymore
        image.Dispose();

        // Create a Tensor from the image data
        Tensor input = new Tensor(1, 64, 64, 1);

        // Copy data from the buffer to the Tensor
        for (int y = 0; y < 64; y++)
        {
            for (int x = 0; x < 64; x++)
            {
                // Get pixel value and normalize (if necessary)
                float pixelValue = buffer[y * 64 + x] / 255f;
                input[0, y, x, 0] = pixelValue;
            }
        }

        // Dispose of the buffer
        buffer.Dispose();

        // Run the model
        worker.Execute(input);
        Debug.Log("Model executed.");

        // Get the output
        Tensor output = worker.PeekOutput();

        // Interpret the output
        InterpretModelOutput(output);

        // Dispose tensors
        input.Dispose();
        output.Dispose();

        yield return null;
    }

    private void InterpretModelOutput(Tensor output)
    {
        // Get the index of the highest probability
        int predictedEmotion = output.ArgMax()[1];

        // Map the index to an emotion label
        string emotion = MapEmotion(predictedEmotion);

        Debug.Log("Detected Emotion: " + emotion);

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
            return "Unknown";
        }
    }
}
