/*******************************************************************************
  Turnstone Biologics Confidential
  
  2018 Turnstone Biologics
  All Rights Reserved.
  
  This file is subject to the terms and conditions defined in
  file 'license.txt', which is part of this source code package.
   
  Contributors :
        Turnstone Biologics - General Release
 ******************************************************************************/
/** 
 * Base class of all application service classes.
 *
 * @author 
 */

using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
	public class HomeController : Controller 
	{			
		/**
		 * redirect to home page
		 */	
	    public IActionResult Index()
        {
            return View();
        }

		/**
		 * redirect to about page
		 */	
	    public IActionResult About()
        {
        	ViewData["Message"] = "demo";
            return View();
        }

		/**
		 * redirect to contact page
		 */	
	    public IActionResult Contact()
        {
            return View();
        }

		/**
		 * redirect to main app page
		 */	
	    public IActionResult RunApp()
        {
            return View("~/Views/Shared/AppPage.cshtml");
        }
	}
}



