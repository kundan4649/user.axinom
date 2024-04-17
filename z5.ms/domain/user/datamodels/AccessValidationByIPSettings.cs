using System;
using System.Collections.Generic;
using System.Text;

namespace z5.ms.domain.user.datamodels
{
    public class AccessValidationByIPSettings
    {
        public int MaxRequestCount { get; set; }
        public int NextAccessAfter { get; set; }
    }
}