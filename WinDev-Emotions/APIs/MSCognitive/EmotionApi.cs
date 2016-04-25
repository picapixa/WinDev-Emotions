using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

/// <summary>
/// Building your own API library all boils down to these two things:
/// - Setting the properties such as the base Uri, parameters, and keys
/// - Using HTTPClient and interpreting the received message
/// 
/// If you want to receive data from third-party APIs and review it without going through
/// the trouble of running this app again and again, play around with the output APIs at
/// http://www.apigee.com.
/// </summary>
namespace WinDev_Emotions.APIs.MSCognitive
{
    /// <summary>
    /// The Emotion API is one of the many APIs by Microsoft Cognitive Services. It
    /// allows you to detect faces and the emotions that each face convey from a photo.
    /// 
    /// Register at microsoft.com/cognitive, paste your subscription key at SubsriptionKey,
    /// get the request URL (BaseUri) from the API Reference, and review it.
    /// 
    /// API Overview: https://www.microsoft.com/cognitive-services/en-us/emotion-api
    /// API Reference: https://dev.projectoxford.ai/docs/services/5639d931ca73072154c1ce89/operations/563b31ea778daf121cc3a5fa
    /// </summary>
    public class EmotionApi
    {
        public const string BaseUri = "https://api.projectoxford.ai/emotion/v1.0/recognize";
        public const string SubscriptionKey = "<your-key-here>";


        /// <summary>
        /// Posts the image to the server. 
        /// 
        /// Returns an exception if the transaction is not successful. Handle this exception
        /// through using a try-catch block and placing this method in the "try" area.
        /// </summary>
        /// <param name="image">The BitmapImage where the Image control is bound.</param>
        /// <param name="bytes">The array representation of the image.</param>
        /// <returns>A JSON string of the result of the </returns>
        public static async Task<string> PostImageAsync(BitmapImage image, byte[] bytes)
        {
            Uri uri = new Uri(BaseUri);

            // The HttpClient is the object that you will use to send data requests to
            // a server and receive a response message to it.
            //
            // Knowledge of the HTTP application protocol is essential in this one,
            // moreso in the HTTP request methods/verbs department.
            //
            // Why not a Wikipedia link? https://en.wikipedia.org/wiki/Hypertext_Transfer_Protocol
            // HttpClient reference: https://blogs.windows.com/buildingapps/2015/11/23/demystifying-httpclient-apis-in-the-universal-windows-platform/

            var client = new HttpClient();

            // Attach your subscription key as part of the request. 
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

            // Retrieve the byte representation of the image from the viewmodel
            byte[] byteData = bytes;

            HttpResponseMessage message = null;

            // Convert the array of bytes to a sendable stream, then submit the POST
            // request to the service.
            using (var bac = new ByteArrayContent(byteData))
            {
                bac.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                message = await client.PostAsync(uri, bac);
            }

            // Read and return the JSON string if successful.
            var content = await message.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            return content;

        }
    }
}
