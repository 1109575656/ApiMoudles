﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.RequestModel
{
    /// <summary>
    /// 登陆实体
    /// </summary>
    public  class SignInModel
    {
        public string LoginName { get; set; }

        public string Password { get; set; }
    }
}