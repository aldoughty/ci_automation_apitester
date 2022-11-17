namespace ci_automation_apitester.Objects
{
    public class TestAttributes
    {
        [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
        public class RequestTest : Attribute
        {
            public string Action;
            public string? Request;
            public int ResponseCode;
            public string Message;
            public string TestName;

            public RequestTest(string action, string request, int responseCode, string message, string testName)
            {
                Action = action;
                Request = request;
                ResponseCode = responseCode;
                Message = message;
                TestName = testName;
            }
        }
        public class RequestAttributeTest
        {
            public string Property { get; set; }
            public string Action { get; set; }
            public string? Request { get; set; }
            public int ResponseCode { get; set; }
            public string Message { get; set; }
            public string TestName { get; set; }

            public RequestAttributeTest Create(string property, string action, string request, int responseCode, string message, string testName)
            {
                Property = property;
                Action = action;
                Request = request;
                ResponseCode = responseCode;
                Message = message;
                TestName = testName;
                return this;
            }
            public RequestAttributeTest Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                RequestAttributeTest copy = JsonConvert.DeserializeObject<RequestAttributeTest>(data);
                return copy;
            }
        }
        [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
        public class UrlTest : Attribute
        {
            public string Action;
            public string? Url;
            public int ResponseCode;
            public string Message;
            public string TestName;

            public UrlTest(string action, string url, int responseCode, string message, string testName)
            {
                Action = action;
                Url = url;
                ResponseCode = responseCode;
                Message = message;
                TestName = testName;
            }
        }
        public class UrlAttributeTest
        {
            public string Property { get; set; }
            public string Action { get; set; }
            public string? Url { get; set; }
            public int ResponseCode { get; set; }
            public string Message { get; set; }
            public string TestName { get; set; }

            public UrlAttributeTest Create(string property, string action, string url, int responseCode, string message, string testName)
            {
                Property = property;
                Action = action;
                Url = url;
                ResponseCode = responseCode;
                Message = message;
                TestName = testName;
                return this;
            }
            public UrlAttributeTest Copy()
            {
                string data = JsonConvert.SerializeObject(this);
                UrlAttributeTest copy = JsonConvert.DeserializeObject<UrlAttributeTest>(data);
                return copy;
            }
        }
    }
}
