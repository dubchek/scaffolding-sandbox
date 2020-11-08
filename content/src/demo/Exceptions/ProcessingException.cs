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
 * Used to indicate an error occurred in generic processing.
 * <p>
 * @author 
 */

namespace demo.Exceptions
{
    /// <summary>
    /// Common  exception thrown during the processing a business request
    /// </summary>
	public class ProcessingException: System.Exception
	{
        /// <summary>
        /// sole constructor
        /// </summary>
        /// <param name="message"></param>
   		public ProcessingException(string message): base(message)
   		{
   		}
	}
}

