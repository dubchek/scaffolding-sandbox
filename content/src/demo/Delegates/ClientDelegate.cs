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
    /// Client business delegate class.
    /// This class implements the Delegate design pattern for the purpose of:
    /// 
    /// 1. Reducing coupling between the business tier and a client of the business tier by hiding all business-tier implementation details</li>
    /// 2. Improving the available of Client related services in the case of a Client business related service failing.</li>
    /// 3. Exposes a simpler, uniform ${ className} interface to the business tier, making it easy for clients to consume a simple Java object.</li>
    /// 4. Hides the communication protocol that may be required to fulfill Client business related services.</li>
    /// @author 
    /// </summary>
	public class ClientDelegate : BaseDelegate
	{	
	
//************************************************************************
// Public Methods
//************************************************************************

        /// <summary>
        /// default constructor, using dependency injection to acquire a ILogger<Client> implementation
        /// <param name="_logger"></para>
        /// </summary>
		public ClientDelegate( ILogger<Client> _logger )
		{
			logger = _logger;
		}

        /// <summmary>
        /// Returns a singleton instance of ClientDelegate(). 
        /// All methods are expected to be stateless and self-sufficient.
        /// <returns></returns>
        /// </summary>
		public static ClientDelegate getClientInstance()
		{
		    if ( singleton == null )
		    {
		    	singleton = new ClientDelegate( logger );
		    }
		    
		    return( singleton );
		}
 
        /// <summmary>
        /// Retrieve the Client via an ClientPrimaryKey.
        /// <param name="key></para>
        /// <returns></returns>
        /// </summary>        
	    public Client getClient( ClientPrimaryKey key ) 
	    {
	    	string msgPrefix = "ClientDelegate:getClient - ";
	        if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        Client returnBO = null;
	                
	        ClientDAO dao = getClientDAO();
	        
	        try
	        {
	            returnBO = dao.findClient( key );
	        }
	        catch( Exception exc )
	        {
	            string errMsg = "ClientDelegate:getClient( ClientPrimaryKey key ) - unable to locate Client with key " + key.ToString() + " - " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw new ProcessingException( errMsg );
	        }
	        finally
	        {
	            releaseClientDAO( dao );
	        }        
	        
	        return returnBO;
	    }
	
	
        /// <summmary>
        /// Retrieve a list of all the Client models
        /// <returns></returns>
        /// </summary>        
        public List<Client> getAllClient() 
	    {
	    	string msgPrefix				= "ClientDelegate:getAllClient() - ";
	        List<Client> array		= null;	        
	        ClientDAO dao 			= getClientDAO();
	    
	        try
	        {
	            array = dao.findAllClient();
	        }
	        catch( Exception exc )
	        {
	            string errMsg = msgPrefix + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseClientDAO( dao );
	        }        
	        
	        return array;
	    }

        /// <summmary>
        /// Interacts with the persistence tier to create (insert) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
		public Client createClient( Client model )
	    {
			string msgPrefix = "ClientDelegate:createClient - ";
			
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        // return value once persisted
	        ClientDAO dao  = getClientDAO();
	        
	        try
	        {
	            model = dao.createClient( model );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = "ClientDelegate:createClient() - Unable to create Client" + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseClientDAO( dao );
	        }        
	        
	        return( model );
	        
	    }
	
        /// <summmary>
        /// Interacts with the persistence tier to save (update) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
        public Client saveClient( Client model ) 
	    {
	    	string msgPrefix = "ClientDelegate:saveClient - ";
	    	
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	                
	        ClientPrimaryKey key = model.getClientPrimaryKey();
	                    
	        if ( key != null )
	        {
	            ClientDAO dao = getClientDAO();
	
	            try
	            {                    
	                model = (Client)dao.saveClient( model );
	            }
	            catch (Exception exc)
	            {
	                string errMsg = "ClientDelegate:saveClient() - Unable to save Client" + exc;
	                logger.LogInformation( errMsg );
	                throw ( new ProcessingException( errMsg  ) );
	            }
	            finally
	            {
	                releaseClientDAO( dao );
	            }
	            
	        }
	        else
	        {
	            string errMsg = "ClientDelegate:saveClient() - Unable to create Client due to it having a null ClientPrimaryKey.";             
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
        public bool delete( ClientPrimaryKey key ) 
	    {    	
	    	string msgPrefix 	= "ClientDelegate:saveClient - ";
	    	bool deleted 	= false;
	    	
			if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException ( errMsg ) );
	        }
	        
	        ClientDAO dao  = getClientDAO();
	
	        try
	        {                    
	            deleted = dao.deleteClient( key );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = msgPrefix + "Unable to delete Client using key = "  + key + ". " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) ); 
	        }
	        finally
	        {
	            releaseClientDAO( dao );
	        }
	        		
	        return deleted;
	    }

////////////////////////////////////////////////////////////////////////////
// internal helper methods
////////////////////////////////////////////////////////////////////////////
    
        /// <summmary>
        /// Returns the Client specific DAO.
        /// <returns></returns>
        /// </summary>        
        private ClientDAO getClientDAO()
	    {
	    	ILoggerFactory logFactory = ApplicationLogger.LoggerFactory;
	        return( new ClientDAO( logFactory.CreateLogger<ClientDAO>() ) ); 
	    }
	
        /// <summary>
        /// nulls the provided  ClientDAO
        /// <param name="dao"></para>
        /// </summary>
	    public void releaseClientDAO( ClientDAO dao )
	    {
	        dao = null;
	    }
        
//************************************************************************
// Attributes
//************************************************************************
	    protected static ClientDelegate singleton = null;
		private static ILogger<Client> logger;
	}    
}



