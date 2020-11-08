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
    /// Converter PrimaryKey class, encapsulating the key field(s) of an associated Converter model 
    /// 
    /// @author    
    /// </summary>
	public class ConverterPrimaryKey
     : BasePrimaryKey
 	{

//************************************************************************
// Public Methods
//************************************************************************
    	
        /// <summary>
        /// default constructor
        /// </summary>
		public ConverterPrimaryKey()
		{
		}
    
        /// <summary>
        /// Constructor with all arguments relating to the primary key
        /// </summary>
	    public ConverterPrimaryKey(    
		long converterId 			
		)
	    {
			ConverterId = converterId;
      	}   

//************************************************************************
// Overload 
//************************************************************************

        /// <summary>
        /// Returns the key or keys associated with a associated Converter model
        /// <returns></returns>
        /// </summary>
	    override public ArrayList keys()
	    {
			// assign the attributes to the Collection back to the parent
	        ArrayList keys = new ArrayList();
	        
		    keys.Add( ConverterId );
 
    	    return( keys );
	    }

        /// <summary>
        /// Returns the first assigned key
        /// <returns></returns>
        /// </summary>
		override public Object getFirstKey()
		{				   
			return( ConverterId );
		}
 
    
//************************************************************************
// Attributes
//************************************************************************

	public long converterId { get; set; }

	}
}


