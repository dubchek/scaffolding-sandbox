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
    /// Encapsulates data for Converter model
    /// 
    /// @author 
    /// </summary>
 public class Converter : Base
{

        //************************************************************************
        // Constructors
        //************************************************************************

        /// <summary>
        /// default constructor
        /// </summary>
        public Converter()
		{
		}

//************************************************************************
// Accessor Methods
//************************************************************************
        /// <summary>
        /// returns the key fields wrapped in a ConverterPrimaryKey
        /// 
        /// <returns></returns>
        /// </summary>
	    public virtual ConverterPrimaryKey getConverterPrimaryKey() 
	    {    
	    	ConverterPrimaryKey key = new ConverterPrimaryKey();
			key.converterId = this.converterId;
    	    return( key );
    	}

        /// <summary>
        /// Perfoms a copy of only the direct attributes but not the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Converter copyShallow( Converter obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Converter:copy(..) - object cannot be null.") );           
	        }
	
	        // Call base class copy
	        base.copy( obj );
        
          this.converterId = obj.converterId;
  
			return this;
    	}

        /// <summary>
        /// Performs a deeper copy which includes copying the direct attributes and the associations
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual Converter copy( Converter obj ) 
	    {
	        if ( obj == null )
	        {
	            throw ( new ProcessingException(" Converter:copy(..) - object cannot be null.") );           
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
	
		returnString = returnString + "converterId = " + this.converterId + ", ";
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
			string identity = "Converter";
			
			identity = identity + "::" ;
			identity = identity + "converterId" ;
	        return ( identity );
	    }

        /// <summary>
        /// Return the model type    
        /// 
        /// <returns></returns>
        public override string getObjectType()
	    {
	        return ("Converter");
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
				
			Converter bo = (Converter)obj;
			
			return( getConverterPrimaryKey().Equals( bo.getConverterPrimaryKey() ) ); 
		}

// attributes
public virtual long converterId { get; set; }
    }
}


