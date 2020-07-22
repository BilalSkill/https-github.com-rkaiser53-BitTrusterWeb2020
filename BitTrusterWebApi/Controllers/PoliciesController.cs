using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BitTrusterWebApi.Services;
using BitTrusterWebApi.Helper;
using Microsoft.Extensions.Options;
using BitTrusterWebApi.Model;
using System.Dynamic;

namespace BitTrusterWebApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _PoliciesService;
        private readonly AppSettings _appSettings;

        public PoliciesController(
           IPolicyService policiesService,
           IOptions<AppSettings> appSettings)
        {
            _PoliciesService = policiesService;
            _appSettings = appSettings.Value;
            DbContext.ConnectionString = _appSettings.ConnectionString;
        }

        [HttpGet]
        public dynamic getPolicies()
        {
            return _PoliciesService.GetPolicies();
        }

        [HttpGet("getpoliciesold")]
        public dynamic getPoliciesOld()
        {
            return _PoliciesService.GetPoliciesOld();
        }

        [HttpPost("setpolicy/{policy}")]
        public dynamic setPolicy(ExpandoObject policy)
        {
            var result = _PoliciesService.SetPolicy(policy);
            return result;
        }

        [HttpDelete("delete/{PolicyID}")]
        public dynamic deletePolicy(int PolicyID)
        {
            return _PoliciesService.DeletePolicy(PolicyID);
        }

        [HttpPost("setpolicyfilter/{obj}")]
        public dynamic SetPolicyFilter(ExpandoObject param)
        {
            var result = _PoliciesService.SetPolicyFilter(param);
            return result;
        }

        [HttpGet("getpolicyfilter/{PolicyID}")]
        public dynamic getPolicyFilter(int PolicyID)
        {
            return _PoliciesService.GetPoliciesFilters(PolicyID);
        }

        [HttpGet("getpolicyfiltervalues")]
        public dynamic getPolicyFilterValues()
        {
            return _PoliciesService.GetPolicyFilterValues();
        }
    }
}
