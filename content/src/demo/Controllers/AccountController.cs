/*******************************************************************************
  Turnstone Biologics Confidential
  
  2018 Turnstone Biologics
  All Rights Reserved.
  
  This file is subject to the terms and conditions defined in
  file 'license.txt', which is part of this source code package.
   
  Contributors :
        Turnstone Biologics - General Release
 ******************************************************************************/


/** 
 * Implements Struts action processing for model Account.
 *
 * @author 
 */


using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using demo.Delegates;
using demo.Exceptions;
using demo.Models;
using demo.PrimaryKeys;

namespace demo.Controllers
{
	public class AccountController : BaseController
	{
		/**
		 * default constructor, using dependency injection to acquire a
		 * ILogger implementation
		 */
		public AccountController( ILogger<AccountController> _logger )
		{
			logger = _logger;
		}

		/**
		 * redirect to the profile .cshtml 
		 */	
	    public IActionResult Profile( string accountId, string action, string parentUrl )
        {
            ViewData["accountId"] = accountId;
            ViewData["action"]			= action;
            ViewData["parentUrl"] 		= parentUrl;
        
            return PartialView("~/Views/AccountProfile.cshtml");
        }
	
		/**
		 * redirect to the list .cshtml 
		 */
	    public IActionResult List( string roleName, string addUrl, string deleteUrl, string modelUrl, string parentUrl )
        {
            ViewData["roleName"] = roleName;
            ViewData["addUrl"] = addUrl;
            ViewData["deleteUrl"] = deleteUrl;
            ViewData["modelUrl"] = modelUrl;
            ViewData["parentUrl"] = parentUrl;

            return PartialView("~/Views/AccountList.cshtml");
        }
	
	    /**
	     * handles saving a Account BO.  if no key provided, calls create, otherwise calls save
	     */
	    public JsonResult save( [FromBody] Account model )
	    {
			account = model;
			
			logger.LogInformation( "Account.save() on - " + model.ToString()  );
			
	        if ( hasPrimaryKey() )
	        {
	            account = update();
	        }
	        else
	        {
	            account = create();
	        }
	        
	        return Json(account);
	    }
	
	    /**
	     * handles updating a Account model
	     */    
	    protected Account update()
	    {
	    	// store provided data
	        Account model = account;
	
	        // load actual data from storage
	        loadHelper( account.getAccountPrimaryKey() );
	    	
	    	// copy provided data into actual data
	    	account.copyShallow( model );
	    	
	        try
	        {                        	        
	            this.account = AccountDelegate.getAccountInstance().saveAccount( account );
	            
	            if ( this.account != null )
	            	logger.LogInformation( "AccountController:update() - successfully updated Account - " + account.ToString() );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "AccountController:update() - successfully update Account - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.account;
	        
	    }
	
	    /**
	     * handles creating a Account model
	     */
	    protected Account create()
	    {
	        try
	        {       
				this.account 	= AccountDelegate.getAccountInstance().createAccount( account );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "AccountController:create() - exception Account - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.account;
	    }
	
	    /**
	     * handles deleting a Account model
	     */
	    public JsonResult delete( String accountId, String[] childIds ) 
	    {                
	        try
	        {
	        	if ( childIds == null || childIds.Length == 0 )
	        	{
	        		long parentId  =  convertToLong( accountId );
	        		AccountDelegate.getAccountInstance().delete( new AccountPrimaryKey( parentId  ) );
	        		logger.LogInformation( "AccountController:delete() - successfully deleted Account with key " + account.getAccountPrimaryKey().keys().ToString());
	        	}
	        	else
	        	{
	        		long tmpId;
	        		foreach( String id in childIds )
	        		{
	        			try
	        			{
	        				tmpId = convertToLong( id );
	        				if ( tmpId != 0 )
	        					AccountDelegate.getAccountInstance().delete( new AccountPrimaryKey( tmpId ) );
	        			}
		                catch( Exception exc )
		                {
		                	signalBadRequest();
	
		                	string errMsg = "AccountController:delete() - " + exc.ToString();
		                	logger.LogError( errMsg );
		                	//throw ( new ProcessingException( errMsg ) );
		                }
	        		}
	        	}
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	string errMsg = "AccountController:delete() - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	//throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return null;
		}        
		
	    /**
	     * handles loading a Account model
	     */    
	    public JsonResult load( String accountId ) 
	    {    	
	        AccountPrimaryKey pk 	= null;
			long id 					= convertToLong( accountId );
			
	    	try
	        {
	    		logger.LogInformation( "Account.load pk is " + id );
	    		
	        	if ( id != 0 )
	        	{
	        		pk = new AccountPrimaryKey( id );
	
	        		loadHelper( pk );
			            
		            logger.LogInformation( "AccountController:load() - successfully loaded - " + this.account.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "AccountController:load() - unable to locate the primary key as an attribute or a selection for - " + account.ToString();				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "AccountController:load() - failed to load Account using Id " + id + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	       return Json(account);
	    }
	
	    /**
	     * handles loading all Account models
	     */
	    public JsonResult loadAll()
	    {                
	        List<Account> accountList = null;
	        
	    	try
	        {                        
	            // load the Account
	            accountList = AccountDelegate.getAccountInstance().getAllAccount();
	            
	            if ( accountList != null && accountList.Count > 0 )
	            	logger.LogInformation(  "AccountController:loadAllAccount() - successfully loaded all Accounts" );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "AccountController:loadAll() - failed to load all Accounts - " + exc.ToString();				
				logger.LogError( errMsg );
	            throw new ProcessingException( errMsg );            
	        }

	       	return Json(accountList);
	                            
	    }
	
// findAllBy methods
 

	    protected Account loadHelper( AccountPrimaryKey pk )
	    {
	    	try
	        {
	    		logger.LogInformation( "Account.loadHelper primary key is " + pk);
	    		
	        	if ( pk != null )
	        	{
	        		// load the contained instance of Account
		            this.account = AccountDelegate.getAccountInstance().getAccount( pk );
		            
		            logger.LogInformation( "AccountController:loadHelper() - successfully loaded - " + this.account.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "AccountController:loadHelper() - null primary key provided.";				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "AccountController:load() - failed to load Account using pk " + pk + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	
	        return account;
	
	    }
	

	    /**
	     * returns true if the account is non-null and has it's primary key field(s) set
	     */
	    protected bool hasPrimaryKey()
	    {
	    	bool hasPK = false;
	
			if ( account != null )
				if ( account.accountId != 0 )
			   hasPK = true;
			
			return( hasPK );
	    }
	
		protected string getSubclassName()
		{ return( "AccountController" ); }
		
		override protected ILogger getLogger()
		{ return( logger ); }

	
//************************************************************************    
// Attributes
//************************************************************************
	    private Account account 			= null;
		private readonly ILogger<AccountController> logger;
	}    
}


