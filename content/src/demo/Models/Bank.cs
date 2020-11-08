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
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using demo.Exceptions;
using demo.PrimaryKeys;

namespace demo.Models
{
    /// <summary>
    /// Encapsulates data for Bank model
    /// 
    /// @author 
    /// </summary>
 public class Bank : Base
{

        //************************************************************************
        // Constructors
        //************************************************************************

        /// <summary>
        /// default constructor
        /// </summary>
        public Bank()
		{
		}

//************************************************************************
// Accessor Methods
//************************************************************************
        /// <summary>
        /// returns the key fields wrapped in a BankPrimaryKey
        /// 
        /// <returns></returns>
        /// </summary>
	    public virtual BankPrimaryKey getBankPrimaryKey() 
	    {    
	    	BankPrimaryKey key = new BankPrimaryKey();
			key.bankId = this.bankId;
    	    return( key );
    	}

        /// <summary>
        /// Perfoms a copy of only the direct attributes but not the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Bank copyShallow( Bank obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Bank:copy(..) - object cannot be null.") );           
	        }
	
	        // Call base class copy
	        base.copy( obj );
        
          this.bankId = obj.bankId;
            this.accounts = obj.accounts;
            this.converter = obj.converter;
            this.cons = obj.cons;
  
			return this;
    	}

        /// <summary>
        /// Performs a deeper copy which includes copying the direct attributes and the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Bank copy( Bank obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Bank:copy(..) - object cannot be null.") );           
	        }
	
	        // Call base class copy
	        base.copy( obj );
	        
	        // shallow copy first
	        copyShallow( obj );
	        
	
			return( this );
	    }

        /// <summary>
        /// returns a string representation of the object model
        /// </summary>
	    public override string ToString()
	    {
	        string returnString = base.ToString() + ", " ;     
	
		returnString = returnString + "bankId = " + this.bankId + ", ";
		returnString = returnString + "accounts = " + this.accounts + ", ";
		returnString = returnString + "converter = " + this.converter + ", ";
		returnString = returnString + "cons = " + this.cons + ", ";
	        return returnString;
	    }

        /// <summary>
        /// Return the names the model is identified by
        /// </summary>
        /// <returns></returns>
public override List<String> getAttributesByNameUserIdentifiesBy()
		{
			List<String> names = new List<String>();
					
			return( names );
		}

        /// <summary>
        /// Return it's unique identity which is a concatenation of the model name 
        /// and the names of its primary key
        /// </summary>
        /// <returns></returns>
        public override String getIdentity()
	    {
			string identity = "Bank";
			
			identity = identity + "::" ;
			identity = identity + "bankId" ;
	        return ( identity );
	    }

        /// <summary>
        /// Return the model type    
        /// 
        /// <returns></returns>
        public override string getObjectType()
	    {
	        return ("Bank");
	    }

        /// <summary>
        /// Compares this instance to the provided Object
        /// <param name="obj"></param>
        /// <returns></returns>
        /// </summary> 
        public override bool equals( Object obj )
		{
		    if (this == obj) 
		        return true;
		        
			if ( obj == null )
				return false;
				
			Bank bo = (Bank)obj;
			
			return( getBankPrimaryKey().Equals( bo.getBankPrimaryKey() ) ); 
		}

// attributes
public virtual long bankId { get; set; }
 public virtual string accounts { get; set; }
 public virtual string converter { get; set; }
 public virtual string cons { get; set; }
    }
}


