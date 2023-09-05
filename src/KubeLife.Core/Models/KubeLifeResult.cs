using System;
using System.Collections.Generic;
using System.Text;

namespace KubeLife.Core.Models
{
    public class KubeLifeResult<T> where T : class
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }

        public KubeLifeResult()
        {   
        }

        public KubeLifeResult(T data)
        {
            Result = data;
            IsSuccess = true;
        }

        public KubeLifeResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public KubeLifeResult(bool isSuccess, string errorMessage, T data)
        {
            IsSuccess = isSuccess;
            Message = errorMessage;
            Result = data;
        }
    }
}
