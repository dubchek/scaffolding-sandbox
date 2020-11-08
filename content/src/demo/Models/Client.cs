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
    /// Encapsulates data for Client model
    /// 
    /// @author 
    /// </summary>
 public class Client : Base
{

        //************************************************************************
        // Constructors
        //************************************************************************

        /// <summary>
        /// default constructor
        /// </summary>
        public Client()
		{
		}

//************************************************************************
// Accessor Methods
//************************************************************************
        /// <summary>
        /// returns the key fields wrapped in a ClientPrimaryKey
        /// 
        /// <returns></returns>
        /// </summary>
	    public virtual ClientPrimaryKey getClientPrimaryKey() 
	    {    
	    	ClientPrimaryKey key = new ClientPrimaryKey();
			key.clientId = this.clientId;
    	    return( key );
    	}

        /// <summary>
        /// Perfoms a copy of only the direct attributes but not the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Client copyShallow( Client obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Client:copy(..) - object cannot be null.") );           
	        }
	
	        // Call base class copy
	        base.copy( obj );
        
          this.clientId = obj.clientId;
            this.id = obj.id;
            this.name = obj.name;
  
			return this;
    	}

        /// <summary>
        /// Performs a deeper copy which includes copying the direct attributes and the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Client copy( Client obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Client:copy(..) - object cannot be null.") );           
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
	
		returnString = returnString + "clientId = " + this.clientId + ", ";
		returnString = returnString + "id = " + this.id + ", ";
		returnString = returnString + "name = " + this.name + ", ";
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
			string identity = "Client";
			
			identity = identity + "::" ;
			identity = identity + "clientId" ;
	        return ( identity );
	    }

        /// <summary>
        /// Return the model type    
        /// 
        /// <returns></returns>
        public override string getObjectType()
	    {
	        return ("Client");
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
				
			Client bo = (Client)obj;
			
			return( getClientPrimaryKey().Equals( bo.getClientPrimaryKey() ) ); 
		}

// attributes
public virtual long clientId { get; set; }
 public virtual string id { get; set; }
 public virtual string name { get; set; }
    }
}


