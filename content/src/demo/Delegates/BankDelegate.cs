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
    /// Bank business delegate class.
    /// This class implements the Delegate design pattern for the purpose of:
    /// 
    /// 1. Reducing coupling between the business tier and a client of the business tier by hiding all business-tier implementation details</li>
    /// 2. Improving the available of Bank related services in the case of a Bank business related service failing.</li>
    /// 3. Exposes a simpler, uniform ${ className} interface to the business tier, making it easy for clients to consume a simple Java object.</li>
    /// 4. Hides the communication protocol that may be required to fulfill Bank business related services.</li>
    /// @author 
    /// </summary>
	public class BankDelegate : BaseDelegate
	{	
	
//************************************************************************
// Public Methods
//************************************************************************

        /// <summary>
        /// default constructor, using dependency injection to acquire a ILogger<Bank> implementation
        /// <param name="_logger"></para>
        /// </summary>
		public BankDelegate( ILogger<Bank> _logger )
		{
			logger = _logger;
		}

        /// <summmary>
        /// Returns a singleton instance of BankDelegate(). 
        /// All methods are expected to be stateless and self-sufficient.
        /// <returns></returns>
        /// </summary>
		public static BankDelegate getBankInstance()
		{
		    if ( singleton == null )
		    {
		    	singleton = new BankDelegate( logger );
		    }
		    
		    return( singleton );
		}
 
        /// <summmary>
        /// Retrieve the Bank via an BankPrimaryKey.
        /// <param name="key></para>
        /// <returns></returns>
        /// </summary>        
	    public Bank getBank( BankPrimaryKey key ) 
	    {
	    	string msgPrefix = "BankDelegate:getBank - ";
	        if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        Bank returnBO = null;
	                
	        BankDAO dao = getBankDAO();
	        
	        try
	        {
	            returnBO = dao.findBank( key );
	        }
	        catch( Exception exc )
	        {
	            string errMsg = "BankDelegate:getBank( BankPrimaryKey key ) - unable to locate Bank with key " + key.ToString() + " - " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw new ProcessingException( errMsg );
	        }
	        finally
	        {
	            releaseBankDAO( dao );
	        }        
	        
	        return returnBO;
	    }
	
	
        /// <summmary>
        /// Retrieve a list of all the Bank models
        /// <returns></returns>
        /// </summary>        
        public List<Bank> getAllBank() 
	    {
	    	string msgPrefix				= "BankDelegate:getAllBank() - ";
	        List<Bank> array		= null;	        
	        BankDAO dao 			= getBankDAO();
	    
	        try
	        {
	            array = dao.findAllBank();
	        }
	        catch( Exception exc )
	        {
	            string errMsg = msgPrefix + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseBankDAO( dao );
	        }        
	        
	        return array;
	    }

        /// <summmary>
        /// Interacts with the persistence tier to create (insert) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
		public Bank createBank( Bank model )
	    {
			string msgPrefix = "BankDelegate:createBank - ";
			
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        
	        // return value once persisted
	        BankDAO dao  = getBankDAO();
	        
	        try
	        {
	            model = dao.createBank( model );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = "BankDelegate:createBank() - Unable to create Bank" + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	        finally
	        {
	            releaseBankDAO( dao );
	        }        
	        
	        return( model );
	        
	    }
	
        /// <summmary>
        /// Interacts with the persistence tier to save (update) the provided model
        /// <param name="model"></para>
        /// <returns></returns>
        /// </summary>        
        public Bank saveBank( Bank model ) 
	    {
	    	string msgPrefix = "BankDelegate:saveBank - ";
	    	
			if ( model == null )
	        {
	            string errMsg = msgPrefix + "null model provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	                
	        BankPrimaryKey key = model.getBankPrimaryKey();
	                    
	        if ( key != null )
	        {
	            BankDAO dao = getBankDAO();
	
	            try
	            {                    
	                model = (Bank)dao.saveBank( model );
	            }
	            catch (Exception exc)
	            {
	                string errMsg = "BankDelegate:saveBank() - Unable to save Bank" + exc;
	                logger.LogInformation( errMsg );
	                throw ( new ProcessingException( errMsg  ) );
	            }
	            finally
	            {
	                releaseBankDAO( dao );
	            }
	            
	        }
	        else
	        {
	            string errMsg = "BankDelegate:saveBank() - Unable to create Bank due to it having a null BankPrimaryKey.";             
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
        public bool delete( BankPrimaryKey key ) 
	    {    	
	    	string msgPrefix 	= "BankDelegate:saveBank - ";
	    	bool deleted 	= false;
	    	
			if ( key == null )
	        {
	            string errMsg = msgPrefix + "null key provided.";
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException ( errMsg ) );
	        }
	        
	        BankDAO dao  = getBankDAO();
	
	        try
	        {                    
	            deleted = dao.deleteBank( key );
	        }
	        catch (Exception exc)
	        {
	            string errMsg = msgPrefix + "Unable to delete Bank using key = "  + key + ". " + exc.ToString();
	            logger.LogInformation( errMsg );
	            throw ( new ProcessingException( errMsg ) ); 
	        }
	        finally
	        {
	            releaseBankDAO( dao );
	        }
	        		
	        return deleted;
	    }

////////////////////////////////////////////////////////////////////////////
// internal helper methods
////////////////////////////////////////////////////////////////////////////
    
        /// <summmary>
        /// Returns the Bank specific DAO.
        /// <returns></returns>
        /// </summary>        
        private BankDAO getBankDAO()
	    {
	    	ILoggerFactory logFactory = ApplicationLogger.LoggerFactory;
	        return( new BankDAO( logFactory.CreateLogger<BankDAO>() ) ); 
	    }
	
        /// <summary>
        /// nulls the provided  BankDAO
        /// <param name="dao"></para>
        /// </summary>
	    public void releaseBankDAO( BankDAO dao )
	    {
	        dao = null;
	    }
        
//************************************************************************
// Attributes
//************************************************************************
	    protected static BankDelegate singleton = null;
		private static ILogger<Bank> logger;
	}    
}



