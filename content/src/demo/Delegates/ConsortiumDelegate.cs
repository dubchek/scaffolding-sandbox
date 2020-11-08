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
    /// Consortium business delegate class.
    /// This class implements the Delegate design pattern for the purpose of:
    /// 
    /// 1. Reducing coupling between the business tier and a client of the business tier by hiding all business-tier implementation details</li>
    /// 2. Improving the available of Consortium related services in the case of a Consortium business related service failing.</li>
    /// 3. Exposes a simpler, uniform ${ className} interface to the business tier, making it easy for clients to consume a simple Java object.</li>
    /// 4. Hides the communication protocol that may be required to fulfill Consortium business related services.</li>
    /// @author 
    /// </summary>
	public class ConsortiumDelegate : BaseDelegate
	{	
	
//************************************************************************
// Public Methods
//************************************************************************

        /// <summary>
        /// default constructor, using dependency injection to acquire a ILogger<Consortium> implementation
        /// <param name="_logger"></para>
        /// </summary>
		public ConsortiumDelegate( ILogger<Consortium> _logger )
		{
			logger = _logger;
		}

        /// <summmary>
        /// Returns a singleton instance of ConsortiumDelegate(). 
        /// All methods are expected to be stateless and self-sufficient.
        /// <returns></returns>
        /// </summary>
		public static ConsortiumDelegate getConsortiumInstance()
		{
		    if ( singleton == null )
		    {
		    	singleton = new ConsortiumDelegate( logger );
		    }
		    
		    return( singleton );
		}
 
        /// <summmary>
        /// Retrieve the Consortium via an ConsortiumPrimaryKey.
        /// <param name="key></para>
        /// <returns></returns>
        /// </summary>        
	    public Consortium getConsortium( ConsortiumPrimaryKey key ) 
	    {
	    	string msgPrefix = "ConsortiumDelegate:getConsortium - ";
	        if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        Consortium returnBO = null;
	                
	        ConsortiumDAO dao = getConsortiumDAO();
	        
	        try
	        {
	            returnBO = dao.findConsortium( key );
	        }
	        catch( Exception exc )
	        {
	            string errMsg = "ConsortiumDelegate:getConsortium( ConsortiumPrimaryKey key ) - unable to locate Consortium with key " + key.ToString() + " - " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw new ProcessingException( errMsg );
	        }
	        finally
	        {
	            releaseConsortiumDAO( dao );
	        }        
	        
	        return returnBO;
	    }
	
	
        /// <summmary>
        /// Retrieve a list of all the Consortium models
        /// <returns></returns>
        /// </summary>        
        public List<Consortium> getAllConsortium() 
	    {
	    	string msgPrefix				= "ConsortiumDelegate:getAllConsortium() - ";
	        List<Consortium> array		= null;	        
	        ConsortiumDAO dao 			= getConsortiumDAO();
	    
	        try
	        {
	            array = dao.findAllConsortium();
	        }
	        catch( Exception exc )
	        {
	            string errMsg = msgPrefix + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseConsortiumDAO( dao );
	        }        
	        
	        return array;
	    }

        /// <summmary>
        /// Interacts with the persistence tier to create (insert) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
		public Consortium createConsortium( Consortium model )
	    {
			string msgPrefix = "ConsortiumDelegate:createConsortium - ";
			
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        // return value once persisted
	        ConsortiumDAO dao  = getConsortiumDAO();
	        
	        try
	        {
	            model = dao.createConsortium( model );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = "ConsortiumDelegate:createConsortium() - Unable to create Consortium" + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseConsortiumDAO( dao );
	        }        
	        
	        return( model );
	        
	    }
	
        /// <summmary>
        /// Interacts with the persistence tier to save (update) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
        public Consortium saveConsortium( Consortium model ) 
	    {
	    	string msgPrefix = "ConsortiumDelegate:saveConsortium - ";
	    	
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	                
	        ConsortiumPrimaryKey key = model.getConsortiumPrimaryKey();
	                    
	        if ( key != null )
	        {
	            ConsortiumDAO dao = getConsortiumDAO();
	
	            try
	            {                    
	                model = (Consortium)dao.saveConsortium( model );
	            }
	            catch (Exception exc)
	            {
	                string errMsg = "ConsortiumDelegate:saveConsortium() - Unable to save Consortium" + exc;
	                logger.LogInformation( errMsg );
	                throw ( new ProcessingException( errMsg  ) );
	            }
	            finally
	            {
	                releaseConsortiumDAO( dao );
	            }
	            
	        }
	        else
	        {
	            string errMsg = "ConsortiumDelegate:saveConsortium() - Unable to create Consortium due to it having a null ConsortiumPrimaryKey.";             
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
        public bool delete( ConsortiumPrimaryKey key ) 
	    {    	
	    	string msgPrefix 	= "ConsortiumDelegate:saveConsortium - ";
	    	bool deleted 	= false;
	    	
			if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException ( errMsg ) );
	        }
	        
	        ConsortiumDAO dao  = getConsortiumDAO();
	
	        try
	        {                    
	            deleted = dao.deleteConsortium( key );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = msgPrefix + "Unable to delete Consortium using key = "  + key + ". " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) ); 
	        }
	        finally
	        {
	            releaseConsortiumDAO( dao );
	        }
	        		
	        return deleted;
	    }

////////////////////////////////////////////////////////////////////////////
// internal helper methods
////////////////////////////////////////////////////////////////////////////
    
        /// <summmary>
        /// Returns the Consortium specific DAO.
        /// <returns></returns>
        /// </summary>        
        private ConsortiumDAO getConsortiumDAO()
	    {
	    	ILoggerFactory logFactory = ApplicationLogger.LoggerFactory;
	        return( new ConsortiumDAO( logFactory.CreateLogger<ConsortiumDAO>() ) ); 
	    }
	
        /// <summary>
        /// nulls the provided  ConsortiumDAO
        /// <param name="dao"></para>
        /// </summary>
	    public void releaseConsortiumDAO( ConsortiumDAO dao )
	    {
	        dao = null;
	    }
        
//************************************************************************
// Attributes
//************************************************************************
	    protected static ConsortiumDelegate singleton = null;
		private static ILogger<Consortium> logger;
	}    
}



