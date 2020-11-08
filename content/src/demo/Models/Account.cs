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
    /// Encapsulates data for Account model
    /// 
    /// @author 
    /// </summary>
 public class Account : Base
{

        //************************************************************************
        // Constructors
        //************************************************************************

        /// <summary>
        /// default constructor
        /// </summary>
        public Account()
		{
		}

//************************************************************************
// Accessor Methods
//************************************************************************
        /// <summary>
        /// returns the key fields wrapped in a AccountPrimaryKey
        /// 
        /// <returns></returns>
        /// </summary>
	    public virtual AccountPrimaryKey getAccountPrimaryKey() 
	    {    
	    	AccountPrimaryKey key = new AccountPrimaryKey();
			key.accountId = this.accountId;
    	    return( key );
    	}

        /// <summary>
        /// Perfoms a copy of only the direct attributes but not the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Account copyShallow( Account obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Account:copy(..) - object cannot be null.") );           
	        }
	
	        // Call base class copy
	        base.copy( obj );
        
          this.accountId = obj.accountId;
            this.id = obj.id;
            this.amount = obj.amount;
            this.currency = obj.currency;
  
			return this;
    	}

        /// <summary>
        /// Performs a deeper copy which includes copying the direct attributes and the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Account copy( Account obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Account:copy(..) - object cannot be null.") );           
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
	
		returnString = returnString + "accountId = " + this.accountId + ", ";
		returnString = returnString + "id = " + this.id + ", ";
		returnString = returnString + "amount = " + this.amount + ", ";
		returnString = returnString + "currency = " + this.currency + ", ";
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
			string identity = "Account";
			
			identity = identity + "::" ;
			identity = identity + "accountId" ;
	        return ( identity );
	    }

        /// <summary>
        /// Return the model type    
        /// 
        /// <returns></returns>
        public override string getObjectType()
	    {
	        return ("Account");
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
				
			Account bo = (Account)obj;
			
			return( getAccountPrimaryKey().Equals( bo.getAccountPrimaryKey() ) ); 
		}

// attributes
public virtual long accountId { get; set; }
 public virtual string id { get; set; }
 public virtual string amount { get; set; }
 public virtual string currency { get; set; }
    }
}


