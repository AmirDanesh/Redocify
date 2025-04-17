using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redocify.Configurations
{
    public class RedocifyConfigs
    {
        public string LaunchRoute { get; set; } = "/redoc";
        public string SwaggerUrl { get; set; } = "/swagger/v1/swagger.json";
    }
}
