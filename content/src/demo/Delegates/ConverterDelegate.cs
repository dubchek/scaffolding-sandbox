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

using demo;
using demo.DAOs;
using demo.Exceptions;
using demo.Models;
using demo.PrimaryKeys;

namespace demo.Delegates
{
    /// <summary>
    /// Converter business delegate class.
    /// This class implements the Delegate design pattern for the purpose of:
    /// 
    /// 1. Reducing coupling between the business tier and a client of the business tier by hiding all business-tier implementation details</li>
    /// 2. Improving the available of Converter related services in the case of a Converter business related service failing.</li>
    /// 3. Exposes a simpler, uniform ${ className} interface to the business tier, making it easy for clients to consume a simple Java object.</li>
    /// 4. Hides the communication protocol that may be required to fulfill Converter business related services.</li>
    /// @author 
    /// </summary>
	public class ConverterDelegate : BaseDelegate
	{	
	
//************************************************************************
// Public Methods
//************************************************************************

        /// <summary>
        /// default constructor, using dependency injection to acquire a ILogger<Converter> implementation
        /// <param name="_logger"></para>
        /// </summary>
		public ConverterDelegate( ILogger<Converter> _logger )
		{
			logger = _logger;
		}

        /// <summmary>
        /// Returns a singleton instance of ConverterDelegate(). 
        /// All methods are expected to be stateless and self-sufficient.
        /// <returns></returns>
        /// </summary>
		public static ConverterDelegate getConverterInstance()
		{
		    if ( singleton == null )
		    {
		    	singleton = new ConverterDelegate( logger );
		    }
		    
		    return( singleton );
		}
 
        /// <summmary>
        /// Retrieve the Converter via an ConverterPrimaryKey.
        /// <param name="key></para>
        /// <returns></returns>
        /// </summary>        
	    public Converter getConverter( ConverterPrimaryKey key ) 
	    {
	    	string msgPrefix = "ConverterDelegate:getConverter - ";
	        if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        Converter returnBO = null;
	                
	        ConverterDAO dao = getConverterDAO();
	        
	        try
	        {
	            returnBO = dao.findConverter( key );
	        }
	        catch( Exception exc )
	        {
	            string errMsg = "ConverterDelegate:getConverter( ConverterPrimaryKey key ) - unable to locate Converter with key " + key.ToString() + " - " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw new ProcessingException( errMsg );
	        }
	        finally
	        {
	            releaseConverterDAO( dao );
	        }        
	        
	        return returnBO;
	    }
	
	
        /// <summmary>
        /// Retrieve a list of all the Converter models
        /// <returns></returns>
        /// </summary>        
        public List<Converter> getAllConverter() 
	    {
	    	string msgPrefix				= "ConverterDelegate:getAllConverter() - ";
	        List<Converter> array		= null;	        
	        ConverterDAO dao 			= getConverterDAO();
	    
	        try
	        {
	            array = dao.findAllConverter();
	        }
	        catch( Exception exc )
	        {
	            string errMsg = msgPrefix + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseConverterDAO( dao );
	        }        
	        
	        return array;
	    }

        /// <summmary>
        /// Interacts with the persistence tier to create (insert) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
		public Converter createConverter( Converter model )
	    {
			string msgPrefix = "ConverterDelegate:createConverter - ";
			
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        // return value once persisted
	        ConverterDAO dao  = getConverterDAO();
	        
	        try
	        {
	            model = dao.createConverter( model );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = "ConverterDelegate:createConverter() - Unable to create Converter" + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseConverterDAO( dao );
	        }        
	        
	        return( model );
	        
	    }
	
        /// <summmary>
        /// Interacts with the persistence tier to save (update) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
        public Converter saveConverter( Converter model ) 
	    {
	    	string msgPrefix = "ConverterDelegate:saveConverter - ";
	    	
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	                
	        ConverterPrimaryKey key = model.getConverterPrimaryKey();
	                    
	        if ( key != null )
	        {
	            ConverterDAO dao = getConverterDAO();
	
	            try
	            {                    
	                model = (Converter)dao.saveConverter( model );
	            }
	            catch (Exception exc)
	            {
	                string errMsg = "ConverterDelegate:saveConverter() - Unable to save Converter" + exc;
	                logger.LogInformation( errMsg );
	                throw ( new ProcessingException( errMsg  ) );
	            }
	            finally
	            {
	                releaseConverterDAO( dao );
	            }
	            
	        }
	        else
	        {
	            string errMsg = "ConverterDelegate:saveConverter() - Unable to create Converter due to it having a null ConverterPrimaryKey.";             
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
			        
	        return( model );
	        
	    }
	   
        /// <summmary>
        /// Deletes the associated model using the provided primary key
        /// <param name="key"></para>
        /// <returns></returns>
        /// </summary>        
        public bool delete( ConverterPrimaryKey key ) 
	    {    	
	    	string msgPrefix 	= "ConverterDelegate:saveConverter - ";
	    	bool deleted 	= false;
	    	
			if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException ( errMsg ) );
	        }
	        
	        ConverterDAO dao  = getConverterDAO();
	
	        try
	        {                    
	            deleted = dao.deleteConverter( key );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = msgPrefix + "Unable to delete Converter using key = "  + key + ". " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) ); 
	        }
	        finally
	        {
	            releaseConverterDAO( dao );
	        }
	        		
	        return deleted;
	    }

////////////////////////////////////////////////////////////////////////////
// internal helper methods
////////////////////////////////////////////////////////////////////////////
    
        /// <summmary>
        /// Returns the Converter specific DAO.
        /// <returns></returns>
        /// </summary>        
        private ConverterDAO getConverterDAO()
	    {
	    	ILoggerFactory logFactory = ApplicationLogger.LoggerFactory;
	        return( new ConverterDAO( logFactory.CreateLogger<ConverterDAO>() ) ); 
	    }
	
        /// <summary>
        /// nulls the provided  ConverterDAO
        /// <param name="dao"></para>
        /// </summary>
	    public void releaseConverterDAO( ConverterDAO dao )
	    {
	        dao = null;
	    }
        
//************************************************************************
// Attributes
//************************************************************************
	    protected static ConverterDelegate singleton = null;
		private static ILogger<Converter> logger;
	}    
}



