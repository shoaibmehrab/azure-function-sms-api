using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Vonage;
using Vonage.Request;


namespace AfSmsApi
{
    public static class HttpTriggerSms
    {
        // It function returns a simple message 

        // abc.com/api/helloazure
        [Function("helloazure")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("helloazure");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }


        // It returns a message with parameter of name from endpoint

        // abc.com/api/hellomyself/john
        [Function("hellomyself/{name}")]

        public static HttpResponseData Hello([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, 
            FunctionContext executionContext, string name)
        {
            var logger = executionContext.GetLogger("hellomyself");
            logger.LogInformation("Hello myself triggered");

            var res = req.CreateResponse(HttpStatusCode.OK);
            res.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            res.WriteString($"Hello {name} from azure function."); 

            return res;
        }

        // It sends a message with number and name

        // api/sms/880152*******/smith
        [Function("sms/{number}/{name}")]

        public static HttpResponseData SendSms([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            FunctionContext executionContext, string name, string number)
        {
            var logger = executionContext.GetLogger("hellomyself");
            logger.LogInformation("Hello myself triggered");

            var res = req.CreateResponse(HttpStatusCode.OK);

            var credentials = Credentials.FromApiKeyAndSecret(
                                                                "yourApiKey",
                                                                "yourApiSecret"
                                                                );

            var VonageClient = new VonageClient(credentials);

            var response = VonageClient.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest()
            {
                To = number,
                From = "AzureF",
                Text = $"Hello {name} from Azure Function Http Trigger",
            });

                
            res.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            res.WriteString($"SMS is successfully sent to {name}.");
            return res;
          
            
        }
    }
}
