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
 * Implements Struts action processing for model Bank.
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
	public class BankController : BaseController
	{
		/**
		 * default constructor, using dependency injection to acquire a
		 * ILogger implementation
		 */
		public BankController( ILogger<BankController> _logger )
		{
			logger = _logger;
		}

		/**
		 * redirect to the profile .cshtml 
		 */	
	    public IActionResult Profile( string bankId, string action, string parentUrl )
        {
            ViewData["bankId"] = bankId;
            ViewData["action"]			= action;
            ViewData["parentUrl"] 		= parentUrl;
        
            return PartialView("~/Views/BankProfile.cshtml");
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

            return PartialView("~/Views/BankList.cshtml");
        }
	
	    /**
	     * handles saving a Bank BO.  if no key provided, calls create, otherwise calls save
	     */
	    public JsonResult save( [FromBody] Bank model )
	    {
			bank = model;
			
			logger.LogInformation( "Bank.save() on - " + model.ToString()  );
			
	        if ( hasPrimaryKey() )
	        {
	            bank = update();
	        }
	        else
	        {
	            bank = create();
	        }
	        
	        return Json(bank);
	    }
	
	    /**
	     * handles updating a Bank model
	     */    
	    protected Bank update()
	    {
	    	// store provided data
	        Bank model = bank;
	
	        // load actual data from storage
	        loadHelper( bank.getBankPrimaryKey() );
	    	
	    	// copy provided data into actual data
	    	bank.copyShallow( model );
	    	
	        try
	        {                        	        
	            this.bank = BankDelegate.getBankInstance().saveBank( bank );
	            
	            if ( this.bank != null )
	            	logger.LogInformation( "BankController:update() - successfully updated Bank - " + bank.ToString() );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "BankController:update() - successfully update Bank - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.bank;
	        
	    }
	
	    /**
	     * handles creating a Bank model
	     */
	    protected Bank create()
	    {
	        try
	        {       
				this.bank 	= BankDelegate.getBankInstance().createBank( bank );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "BankController:create() - exception Bank - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.bank;
	    }
	
	    /**
	     * handles deleting a Bank model
	     */
	    public JsonResult delete( String bankId, String[] childIds ) 
	    {                
	        try
	        {
	        	if ( childIds == null || childIds.Length == 0 )
	        	{
	        		long parentId  =  convertToLong( bankId );
	        		BankDelegate.getBankInstance().delete( new BankPrimaryKey( parentId  ) );
	        		logger.LogInformation( "BankController:delete() - successfully deleted Bank with key " + bank.getBankPrimaryKey().keys().ToString());
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
	        					BankDelegate.getBankInstance().delete( new BankPrimaryKey( tmpId ) );
	        			}
		                catch( Exception exc )
		                {
		                	signalBadRequest();
	
		                	string errMsg = "BankController:delete() - " + exc.ToString();
		                	logger.LogError( errMsg );
		                	//throw ( new ProcessingException( errMsg ) );
		                }
	        		}
	        	}
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	string errMsg = "BankController:delete() - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	//throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return null;
		}        
		
	    /**
	     * handles loading a Bank model
	     */    
	    public JsonResult load( String bankId ) 
	    {    	
	        BankPrimaryKey pk 	= null;
			long id 					= convertToLong( bankId );
			
	    	try
	        {
	    		logger.LogInformation( "Bank.load pk is " + id );
	    		
	        	if ( id != 0 )
	        	{
	        		pk = new BankPrimaryKey( id );
	
	        		loadHelper( pk );
			            
		            logger.LogInformation( "BankController:load() - successfully loaded - " + this.bank.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "BankController:load() - unable to locate the primary key as an attribute or a selection for - " + bank.ToString();				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "BankController:load() - failed to load Bank using Id " + id + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	       return Json(bank);
	    }
	
	    /**
	     * handles loading all Bank models
	     */
	    public JsonResult loadAll()
	    {                
	        List<Bank> bankList = null;
	        
	    	try
	        {                        
	            // load the Bank
	            bankList = BankDelegate.getBankInstance().getAllBank();
	            
	            if ( bankList != null && bankList.Count > 0 )
	            	logger.LogInformation(  "BankController:loadAllBank() - successfully loaded all Banks" );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "BankController:loadAll() - failed to load all Banks - " + exc.ToString();				
				logger.LogError( errMsg );
	            throw new ProcessingException( errMsg );            
	        }

	       	return Json(bankList);
	                            
	    }
	
// findAllBy methods
 

	    protected Bank loadHelper( BankPrimaryKey pk )
	    {
	    	try
	        {
	    		logger.LogInformation( "Bank.loadHelper primary key is " + pk);
	    		
	        	if ( pk != null )
	        	{
	        		// load the contained instance of Bank
		            this.bank = BankDelegate.getBankInstance().getBank( pk );
		            
		            logger.LogInformation( "BankController:loadHelper() - successfully loaded - " + this.bank.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "BankController:loadHelper() - null primary key provided.";				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "BankController:load() - failed to load Bank using pk " + pk + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	
	        return bank;
	
	    }
	

	    /**
	     * returns true if the bank is non-null and has it's primary key field(s) set
	     */
	    protected bool hasPrimaryKey()
	    {
	    	bool hasPK = false;
	
			if ( bank != null )
				if ( bank.bankId != 0 )
			   hasPK = true;
			
			return( hasPK );
	    }
	
		protected string getSubclassName()
		{ return( "BankController" ); }
		
		override protected ILogger getLogger()
		{ return( logger ); }

	
//************************************************************************    
// Attributes
//************************************************************************
	    private Bank bank 			= null;
		private readonly ILogger<BankController> logger;
	}    
}


