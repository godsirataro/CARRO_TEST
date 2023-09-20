namespace CARRO_API.Models
{
    public class ResponseResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; }
        public List<string> Errors { get; set; }
        public int Status { get; set; }
        public T Data { get; set; }

        public static ResponseResult<T> Success<T>(T data)
        {
            return new ResponseResult<T>() { Data = data, IsSuccess = true, Status = 200 };
        }

        public static ResponseResult<T> Success()
        {
            return new ResponseResult<T>() { IsSuccess = true, Status = 200 };
        }

        public static ResponseResult<T> Fail<T>(T data, int status)
        {
            return new ResponseResult<T>() { Data = data, IsSuccess = false, Status = status };
        }

        public static ResponseResult<List<string>> Fail(List<string> errors)
        {
            return new ResponseResult<List<string>>() { Errors = errors, IsSuccess = false, Status = 400 };
        }

        public static ResponseResult<T> Fail()
        {
            return new ResponseResult<T>() { IsSuccess = false, Status = 400 };
        }

        public static ResponseResult<T> Fail(string msg)
        {
            return new ResponseResult<T>() { IsSuccess = false, Status = 500, Error = msg };
        }

        public static ResponseResult<T> Fail(string msg, int status)
        {
            if (status == 0)
                status = 400;
            return new ResponseResult<T>() { IsSuccess = false, Status = status, Error = msg };
        }

        public static ResponseResult<T> UnAuth()
        {
            return new ResponseResult<T>() { IsSuccess = false, Status = 401 };
        }
    }
}
