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
 * Implements Struts action processing for model Client.
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
	public class ClientController : BaseController
	{
		/**
		 * default constructor, using dependency injection to acquire a
		 * ILogger implementation
		 */
		public ClientController( ILogger<ClientController> _logger )
		{
			logger = _logger;
		}

		/**
		 * redirect to the profile .cshtml 
		 */	
	    public IActionResult Profile( string clientId, string action, string parentUrl )
        {
            ViewData["clientId"] = clientId;
            ViewData["action"]			= action;
            ViewData["parentUrl"] 		= parentUrl;
        
            return PartialView("~/Views/ClientProfile.cshtml");
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

            return PartialView("~/Views/ClientList.cshtml");
        }
	
	    /**
	     * handles saving a Client BO.  if no key provided, calls create, otherwise calls save
	     */
	    public JsonResult save( [FromBody] Client model )
	    {
			client = model;
			
			logger.LogInformation( "Client.save() on - " + model.ToString()  );
			
	        if ( hasPrimaryKey() )
	        {
	            client = update();
	        }
	        else
	        {
	            client = create();
	        }
	        
	        return Json(client);
	    }
	
	    /**
	     * handles updating a Client model
	     */    
	    protected Client update()
	    {
	    	// store provided data
	        Client model = client;
	
	        // load actual data from storage
	        loadHelper( client.getClientPrimaryKey() );
	    	
	    	// copy provided data into actual data
	    	client.copyShallow( model );
	    	
	        try
	        {                        	        
	            this.client = ClientDelegate.getClientInstance().saveClient( client );
	            
	            if ( this.client != null )
	            	logger.LogInformation( "ClientController:update() - successfully updated Client - " + client.ToString() );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "ClientController:update() - successfully update Client - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.client;
	        
	    }
	
	    /**
	     * handles creating a Client model
	     */
	    protected Client create()
	    {
	        try
	        {       
				this.client 	= ClientDelegate.getClientInstance().createClient( client );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "ClientController:create() - exception Client - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.client;
	    }
	
	    /**
	     * handles deleting a Client model
	     */
	    public JsonResult delete( String clientId, String[] childIds ) 
	    {                
	        try
	        {
	        	if ( childIds == null || childIds.Length == 0 )
	        	{
	        		long parentId  =  convertToLong( clientId );
	        		ClientDelegate.getClientInstance().delete( new ClientPrimaryKey( parentId  ) );
	        		logger.LogInformation( "ClientController:delete() - successfully deleted Client with key " + client.getClientPrimaryKey().keys().ToString());
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
	        					ClientDelegate.getClientInstance().delete( new ClientPrimaryKey( tmpId ) );
	        			}
		                catch( Exception exc )
		                {
		                	signalBadRequest();
	
		                	string errMsg = "ClientController:delete() - " + exc.ToString();
		                	logger.LogError( errMsg );
		                	//throw ( new ProcessingException( errMsg ) );
		                }
	        		}
	        	}
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	string errMsg = "ClientController:delete() - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	//throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return null;
		}        
		
	    /**
	     * handles loading a Client model
	     */    
	    public JsonResult load( String clientId ) 
	    {    	
	        ClientPrimaryKey pk 	= null;
			long id 					= convertToLong( clientId );
			
	    	try
	        {
	    		logger.LogInformation( "Client.load pk is " + id );
	    		
	        	if ( id != 0 )
	        	{
	        		pk = new ClientPrimaryKey( id );
	
	        		loadHelper( pk );
			            
		            logger.LogInformation( "ClientController:load() - successfully loaded - " + this.client.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "ClientController:load() - unable to locate the primary key as an attribute or a selection for - " + client.ToString();				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ClientController:load() - failed to load Client using Id " + id + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	       return Json(client);
	    }
	
	    /**
	     * handles loading all Client models
	     */
	    public JsonResult loadAll()
	    {                
	        List<Client> clientList = null;
	        
	    	try
	        {                        
	            // load the Client
	            clientList = ClientDelegate.getClientInstance().getAllClient();
	            
	            if ( clientList != null && clientList.Count > 0 )
	            	logger.LogInformation(  "ClientController:loadAllClient() - successfully loaded all Clients" );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ClientController:loadAll() - failed to load all Clients - " + exc.ToString();				
				logger.LogError( errMsg );
	            throw new ProcessingException( errMsg );            
	        }

	       	return Json(clientList);
	                            
	    }
	
// findAllBy methods
 

	    protected Client loadHelper( ClientPrimaryKey pk )
	    {
	    	try
	        {
	    		logger.LogInformation( "Client.loadHelper primary key is " + pk);
	    		
	        	if ( pk != null )
	        	{
	        		// load the contained instance of Client
		            this.client = ClientDelegate.getClientInstance().getClient( pk );
		            
		            logger.LogInformation( "ClientController:loadHelper() - successfully loaded - " + this.client.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "ClientController:loadHelper() - null primary key provided.";				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ClientController:load() - failed to load Client using pk " + pk + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	
	        return client;
	
	    }
	

	    /**
	     * returns true if the client is non-null and has it's primary key field(s) set
	     */
	    protected bool hasPrimaryKey()
	    {
	    	bool hasPK = false;
	
			if ( client != null )
				if ( client.clientId != 0 )
			   hasPK = true;
			
			return( hasPK );
	    }
	
		protected string getSubclassName()
		{ return( "ClientController" ); }
		
		override protected ILogger getLogger()
		{ return( logger ); }

	
//************************************************************************    
// Attributes
//************************************************************************
	    private Client client 			= null;
		private readonly ILogger<ClientController> logger;
	}    
}


