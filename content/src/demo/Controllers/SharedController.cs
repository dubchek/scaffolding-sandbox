/*******************************************************************************
  Turnstone Biologics Confidential
  
  2018 Turnstone Biologics
  All Rights Reserved.
  
  This file is subject to the terms and conditions defined in
  file 'license.txt', which is part of this source code package.
   
  Contributors :
        Turnstone Biologics - General Release
 ******************************************************************************/
using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

/** 
 * Base class of all application service classes.
 *
 * @author 
 */
namespace demo.Controllers
{
	public class SharedController : Controller 
	{			
		/**
		 * redirect to rended the Shared MultiSelect view
		 */	
	    public IActionResult MultiSelect( string sourceUrl, string modelUrl, string roleName, string value, string text )
        {
            ViewData["sourceUrl"] 	= sourceUrl;
            ViewData["modelUrl"]	= modelUrl;
            ViewData["roleName"] 	= roleName;
        	ViewData["value"] 		= value;
        	ViewData["text"] 		= text;
        
            return PartialView("~/Views/Shared/MultiSelect.cshtml");
        }
	}
}



