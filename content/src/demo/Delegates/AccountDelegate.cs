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
    /// Account business delegate class.
    /// This class implements the Delegate design pattern for the purpose of:
    /// 
    /// 1. Reducing coupling between the business tier and a client of the business tier by hiding all business-tier implementation details</li>
    /// 2. Improving the available of Account related services in the case of a Account business related service failing.</li>
    /// 3. Exposes a simpler, uniform ${ className} interface to the business tier, making it easy for clients to consume a simple Java object.</li>
    /// 4. Hides the communication protocol that may be required to fulfill Account business related services.</li>
    /// @author 
    /// </summary>
	public class AccountDelegate : BaseDelegate
	{	
	
//************************************************************************
// Public Methods
//************************************************************************

        /// <summary>
        /// default constructor, using dependency injection to acquire a ILogger<Account> implementation
        /// <param name="_logger"></para>
        /// </summary>
		public AccountDelegate( ILogger<Account> _logger )
		{
			logger = _logger;
		}

        /// <summmary>
        /// Returns a singleton instance of AccountDelegate(). 
        /// All methods are expected to be stateless and self-sufficient.
        /// <returns></returns>
        /// </summary>
		public static AccountDelegate getAccountInstance()
		{
		    if ( singleton == null )
		    {
		    	singleton = new AccountDelegate( logger );
		    }
		    
		    return( singleton );
		}
 
        /// <summmary>
        /// Retrieve the Account via an AccountPrimaryKey.
        /// <param name="key></para>
        /// <returns></returns>
        /// </summary>        
	    public Account getAccount( AccountPrimaryKey key ) 
	    {
	    	string msgPrefix = "AccountDelegate:getAccount - ";
	        if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        Account returnBO = null;
	                
	        AccountDAO dao = getAccountDAO();
	        
	        try
	        {
	            returnBO = dao.findAccount( key );
	        }
	        catch( Exception exc )
	        {
	            string errMsg = "AccountDelegate:getAccount( AccountPrimaryKey key ) - unable to locate Account with key " + key.ToString() + " - " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw new ProcessingException( errMsg );
	        }
	        finally
	        {
	            releaseAccountDAO( dao );
	        }        
	        
	        return returnBO;
	    }
	
	
        /// <summmary>
        /// Retrieve a list of all the Account models
        /// <returns></returns>
        /// </summary>        
        public List<Account> getAllAccount() 
	    {
	    	string msgPrefix				= "AccountDelegate:getAllAccount() - ";
	        List<Account> array		= null;	        
	        AccountDAO dao 			= getAccountDAO();
	    
	        try
	        {
	            array = dao.findAllAccount();
	        }
	        catch( Exception exc )
	        {
	            string errMsg = msgPrefix + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseAccountDAO( dao );
	        }        
	        
	        return array;
	    }

        /// <summmary>
        /// Interacts with the persistence tier to create (insert) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
		public Account createAccount( Account model )
	    {
			string msgPrefix = "AccountDelegate:createAccount - ";
			
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        // return value once persisted
	        AccountDAO dao  = getAccountDAO();
	        
	        try
	        {
	            model = dao.createAccount( model );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = "AccountDelegate:createAccount() - Unable to create Account" + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseAccountDAO( dao );
	        }        
	        
	        return( model );
	        
	    }
	
        /// <summmary>
        /// Interacts with the persistence tier to save (update) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
        public Account saveAccount( Account model ) 
	    {
	    	string msgPrefix = "AccountDelegate:saveAccount - ";
	    	
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	                
	        AccountPrimaryKey key = model.getAccountPrimaryKey();
	                    
	        if ( key != null )
	        {
	            AccountDAO dao = getAccountDAO();
	
	            try
	            {                    
	                model = (Account)dao.saveAccount( model );
	            }
	            catch (Exception exc)
	            {
	                string errMsg = "AccountDelegate:saveAccount() - Unable to save Account" + exc;
	                logger.LogInformation( errMsg );
	                throw ( new ProcessingException( errMsg  ) );
	            }
	            finally
	            {
	                releaseAccountDAO( dao );
	            }
	            
	        }
	        else
	        {
	            string errMsg = "AccountDelegate:saveAccount() - Unable to create Account due to it having a null AccountPrimaryKey.";             
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
        public bool delete( AccountPrimaryKey key ) 
	    {    	
	    	string msgPrefix 	= "AccountDelegate:saveAccount - ";
	    	bool deleted 	= false;
	    	
			if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException ( errMsg ) );
	        }
	        
	        AccountDAO dao  = getAccountDAO();
	
	        try
	        {                    
	            deleted = dao.deleteAccount( key );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = msgPrefix + "Unable to delete Account using key = "  + key + ". " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) ); 
	        }
	        finally
	        {
	            releaseAccountDAO( dao );
	        }
	        		
	        return deleted;
	    }

////////////////////////////////////////////////////////////////////////////
// internal helper methods
////////////////////////////////////////////////////////////////////////////
    
        /// <summmary>
        /// Returns the Account specific DAO.
        /// <returns></returns>
        /// </summary>        
        private AccountDAO getAccountDAO()
	    {
	    	ILoggerFactory logFactory = ApplicationLogger.LoggerFactory;
	        return( new AccountDAO( logFactory.CreateLogger<AccountDAO>() ) ); 
	    }
	
        /// <summary>
        /// nulls the provided  AccountDAO
        /// <param name="dao"></para>
        /// </summary>
	    public void releaseAccountDAO( AccountDAO dao )
	    {
	        dao = null;
	    }
        
//************************************************************************
// Attributes
//************************************************************************
	    protected static AccountDelegate singleton = null;
		private static ILogger<Account> logger;
	}    
}



