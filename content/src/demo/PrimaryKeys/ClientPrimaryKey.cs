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

namespace demo.PrimaryKeys
{
    /// <summary>
    /// Client PrimaryKey class, encapsulating the key field(s) of an associated Client model 
    /// 
    /// @author    
    /// </summary>
	public class ClientPrimaryKey
     : BasePrimaryKey
 	{

//************************************************************************
// Public Methods
//************************************************************************
    	
        /// <summary>
        /// default constructor
        /// </summary>
		public ClientPrimaryKey()
		{
		}
    
        /// <summary>
        /// Constructor with all arguments relating to the primary key
        /// </summary>
	    public ClientPrimaryKey(    
		long clientId 			
		)
	    {
			ClientId = clientId;
      	}   

//************************************************************************
// Overload 
//************************************************************************

        /// <summary>
        /// Returns the key or keys associated with a associated Client model
        /// <returns></returns>
        /// </summary>
	    override public ArrayList keys()
	    {
			// assign the attributes to the Collection back to the parent
	        ArrayList keys = new ArrayList();
	        
		    keys.Add( ClientId );
 
    	    return( keys );
	    }

        /// <summary>
        /// Returns the first assigned key
        /// <returns></returns>
        /// </summary>
		override public Object getFirstKey()
		{				   
			return( ClientId );
		}
 
    
//************************************************************************
// Attributes
//************************************************************************

	public long clientId { get; set; }

	}
}


