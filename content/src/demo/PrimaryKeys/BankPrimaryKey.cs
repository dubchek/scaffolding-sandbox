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
    /// Bank PrimaryKey class, encapsulating the key field(s) of an associated Bank model 
    /// 
    /// @author    
    /// </summary>
	public class BankPrimaryKey
     : BasePrimaryKey
 	{

//************************************************************************
// Public Methods
//************************************************************************
    	
        /// <summary>
        /// default constructor
        /// </summary>
		public BankPrimaryKey()
		{
		}
    
        /// <summary>
        /// Constructor with all arguments relating to the primary key
        /// </summary>
	    public BankPrimaryKey(    
		long bankId 			
		)
	    {
			BankId = bankId;
      	}   

//************************************************************************
// Overload 
//************************************************************************

        /// <summary>
        /// Returns the key or keys associated with a associated Bank model
        /// <returns></returns>
        /// </summary>
	    override public ArrayList keys()
	    {
			// assign the attributes to the Collection back to the parent
	        ArrayList keys = new ArrayList();
	        
		    keys.Add( BankId );
 
    	    return( keys );
	    }

        /// <summary>
        /// Returns the first assigned key
        /// <returns></returns>
        /// </summary>
		override public Object getFirstKey()
		{				   
			return( BankId );
		}
 
    
//************************************************************************
// Attributes
//************************************************************************

	public long bankId { get; set; }

	}
}


