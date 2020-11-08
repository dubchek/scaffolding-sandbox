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
    /// Account PrimaryKey class, encapsulating the key field(s) of an associated Account model 
    /// 
    /// @author    
    /// </summary>
	public class AccountPrimaryKey
     : BasePrimaryKey
 	{

//************************************************************************
// Public Methods
//************************************************************************
    	
        /// <summary>
        /// default constructor
        /// </summary>
		public AccountPrimaryKey()
		{
		}
    
        /// <summary>
        /// Constructor with all arguments relating to the primary key
        /// </summary>
	    public AccountPrimaryKey(    
		long accountId 			
		)
	    {
			AccountId = accountId;
      	}   

//************************************************************************
// Overload 
//************************************************************************

        /// <summary>
        /// Returns the key or keys associated with a associated Account model
        /// <returns></returns>
        /// </summary>
	    override public ArrayList keys()
	    {
			// assign the attributes to the Collection back to the parent
	        ArrayList keys = new ArrayList();
	        
		    keys.Add( AccountId );
 
    	    return( keys );
	    }

        /// <summary>
        /// Returns the first assigned key
        /// <returns></returns>
        /// </summary>
		override public Object getFirstKey()
		{				   
			return( AccountId );
		}
 
    
//************************************************************************
// Attributes
//************************************************************************

	public long accountId { get; set; }

	}
}


