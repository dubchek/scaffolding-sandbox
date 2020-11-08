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
using Microsoft.Extensions.Logging;

using demo.Exceptions;
using demo.Models;
using demo.PrimaryKeys;

namespace demo.Controllers
{
	public abstract class BaseController : Controller
	{	
		
		protected long convertToLong( string strValue )
		{ 
			long retVal = 0;
			
			if ( strValue != null )
			{
				try
				{
					retVal = Convert.ToInt64(strValue);
				}
				catch( Exception exc )
				{
					getLogger().LogError( "Failed to convert " + strValue + " to a long " + exc.ToString() );
				}
			}			
			return( retVal );
				
		}
		
		protected void signalBadRequest()
		{}
		
		abstract protected ILogger getLogger();
	}
}



