using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ELearn.Application.Helpers.Response
{
    public static class ResponseHandler
    {
        public static Response<T> Updated<T>(T entity)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Updated Successfully",
                Data = entity
            };
        }
        public static Response<T> Deleted<T>()
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Deleted Successfully"
            };
        }
        public static Response<T> Success<T>(T entity, string message = "succeeded process", object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = message,
                Meta = Meta
            };
        }
        
        /*public static Response<T> Success<T>(string message = "succeeded process", object Meta = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message,
                Meta = Meta
            };
        }*/

        public static Response<ICollection<T>> ManySuccess<T>(ICollection<T> entities, object meta = null)
        {
            return new Response<ICollection<T>>
            {
                Data = entities,
                StatusCode = HttpStatusCode.OK,
                Succeeded = true,
                Message = "Success",
                Meta = meta
            };
        }
        public static Response<T> Unauthorized<T>(string message = "UnAuthorized")
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Succeeded = false,
                Message = message
            };
        }
        public static Response<T> Forbidden<T>(string message = "Forbidden")
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.Forbidden,
                Succeeded = false,
                Message = message
            };
        }
        public static Response<T> BadRequest<T>(string Message = null, List<string> Errors = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = Message == null ? "Bad Request" : Message,
                Errors = Errors
            };
        }

        public static Response<T> NotFound<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? "Not Found" : message
            };
        }

        public static Response<T> Created<T>(T entity, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Entity created",
                Meta = Meta
            };
        }

        public static Response<ICollection<T>> ManyCreated<T>(ICollection<T> entities, object meta = null)
        {
            return new Response<ICollection<T>>
            {
                Data = entities,
                StatusCode = HttpStatusCode.Created,
                Succeeded = true,
                Message = "Entities Created",
                Meta = meta
            };
        }
        
        public static IActionResult CreateResponse<T>(this ControllerBase controllerBase, Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}