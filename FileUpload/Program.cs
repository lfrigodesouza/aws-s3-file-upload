using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.Util;

namespace FileUpload
{
    internal class Program
    {
        private const string bucketName = "[BucketName]";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.SAEast1;
        private static IAmazonS3 client;
        private static BasicAWSCredentials basicCredentials;

        private static void Main(string[] args)
        {
            basicCredentials = new BasicAWSCredentials( "[AccessKey]", "[SecretKey]");
            client = new AmazonS3Client(basicCredentials, bucketRegion);
            UploadFileAsync().Wait();
        }

        private static async Task UploadFileAsync()
        {
            await using var memoryStream = new MemoryStream(); 
            var image = Image.FromFile("sao-paulo.jpg");
            image.Save(memoryStream, image.RawFormat);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = memoryStream,
                Key = "D941DB2DB49957008AF670042013EDD2ED3ACAD960E6826B66D3D2A35FAEE00E",
                ContentType = MediaTypeNames.Image.Jpeg,
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead
            };
            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }
    }
}