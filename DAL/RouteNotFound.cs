using System;

namespace DAL
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException()
        {
        }

        public RouteNotFoundException(string message)
            : base(message)
        {
        }

    }
}
