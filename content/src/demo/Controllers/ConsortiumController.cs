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
 * Implements Struts action processing for model Consortium.
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
	public class ConsortiumController : BaseController
	{
		/**
		 * default constructor, using dependency injection to acquire a
		 * ILogger implementation
		 */
		public ConsortiumController( ILogger<ConsortiumController> _logger )
		{
			logger = _logger;
		}

		/**
		 * redirect to the profile .cshtml 
		 */	
	    public IActionResult Profile( string consortiumId, string action, string parentUrl )
        {
            ViewData["consortiumId"] = consortiumId;
            ViewData["action"]			= action;
            ViewData["parentUrl"] 		= parentUrl;
        
            return PartialView("~/Views/ConsortiumProfile.cshtml");
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

            return PartialView("~/Views/ConsortiumList.cshtml");
        }
	
	    /**
	     * handles saving a Consortium BO.  if no key provided, calls create, otherwise calls save
	     */
	    public JsonResult save( [FromBody] Consortium model )
	    {
			consortium = model;
			
			logger.LogInformation( "Consortium.save() on - " + model.ToString()  );
			
	        if ( hasPrimaryKey() )
	        {
	            consortium = update();
	        }
	        else
	        {
	            consortium = create();
	        }
	        
	        return Json(consortium);
	    }
	
	    /**
	     * handles updating a Consortium model
	     */    
	    protected Consortium update()
	    {
	    	// store provided data
	        Consortium model = consortium;
	
	        // load actual data from storage
	        loadHelper( consortium.getConsortiumPrimaryKey() );
	    	
	    	// copy provided data into actual data
	    	consortium.copyShallow( model );
	    	
	        try
	        {                        	        
	            this.consortium = ConsortiumDelegate.getConsortiumInstance().saveConsortium( consortium );
	            
	            if ( this.consortium != null )
	            	logger.LogInformation( "ConsortiumController:update() - successfully updated Consortium - " + consortium.ToString() );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "ConsortiumController:update() - successfully update Consortium - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.consortium;
	        
	    }
	
	    /**
	     * handles creating a Consortium model
	     */
	    protected Consortium create()
	    {
	        try
	        {       
				this.consortium 	= ConsortiumDelegate.getConsortiumInstance().createConsortium( consortium );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "ConsortiumController:create() - exception Consortium - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.consortium;
	    }
	
	    /**
	     * handles deleting a Consortium model
	     */
	    public JsonResult delete( String consortiumId, String[] childIds ) 
	    {                
	        try
	        {
	        	if ( childIds == null || childIds.Length == 0 )
	        	{
	        		long parentId  =  convertToLong( consortiumId );
	        		ConsortiumDelegate.getConsortiumInstance().delete( new ConsortiumPrimaryKey( parentId  ) );
	        		logger.LogInformation( "ConsortiumController:delete() - successfully deleted Consortium with key " + consortium.getConsortiumPrimaryKey().keys().ToString());
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
	        					ConsortiumDelegate.getConsortiumInstance().delete( new ConsortiumPrimaryKey( tmpId ) );
	        			}
		                catch( Exception exc )
		                {
		                	signalBadRequest();
	
		                	string errMsg = "ConsortiumController:delete() - " + exc.ToString();
		                	logger.LogError( errMsg );
		                	//throw ( new ProcessingException( errMsg ) );
		                }
	        		}
	        	}
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	string errMsg = "ConsortiumController:delete() - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	//throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return null;
		}        
		
	    /**
	     * handles loading a Consortium model
	     */    
	    public JsonResult load( String consortiumId ) 
	    {    	
	        ConsortiumPrimaryKey pk 	= null;
			long id 					= convertToLong( consortiumId );
			
	    	try
	        {
	    		logger.LogInformation( "Consortium.load pk is " + id );
	    		
	        	if ( id != 0 )
	        	{
	        		pk = new ConsortiumPrimaryKey( id );
	
	        		loadHelper( pk );
			            
		            logger.LogInformation( "ConsortiumController:load() - successfully loaded - " + this.consortium.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "ConsortiumController:load() - unable to locate the primary key as an attribute or a selection for - " + consortium.ToString();				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ConsortiumController:load() - failed to load Consortium using Id " + id + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	       return Json(consortium);
	    }
	
	    /**
	     * handles loading all Consortium models
	     */
	    public JsonResult loadAll()
	    {                
	        List<Consortium> consortiumList = null;
	        
	    	try
	        {                        
	            // load the Consortium
	            consortiumList = ConsortiumDelegate.getConsortiumInstance().getAllConsortium();
	            
	            if ( consortiumList != null && consortiumList.Count > 0 )
	            	logger.LogInformation(  "ConsortiumController:loadAllConsortium() - successfully loaded all Consortiums" );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ConsortiumController:loadAll() - failed to load all Consortiums - " + exc.ToString();				
				logger.LogError( errMsg );
	            throw new ProcessingException( errMsg );            
	        }

	       	return Json(consortiumList);
	                            
	    }
	
// findAllBy methods
 

	    protected Consortium loadHelper( ConsortiumPrimaryKey pk )
	    {
	    	try
	        {
	    		logger.LogInformation( "Consortium.loadHelper primary key is " + pk);
	    		
	        	if ( pk != null )
	        	{
	        		// load the contained instance of Consortium
		            this.consortium = ConsortiumDelegate.getConsortiumInstance().getConsortium( pk );
		            
		            logger.LogInformation( "ConsortiumController:loadHelper() - successfully loaded - " + this.consortium.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "ConsortiumController:loadHelper() - null primary key provided.";				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ConsortiumController:load() - failed to load Consortium using pk " + pk + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	
	        return consortium;
	
	    }
	

	    /**
	     * returns true if the consortium is non-null and has it's primary key field(s) set
	     */
	    protected bool hasPrimaryKey()
	    {
	    	bool hasPK = false;
	
			if ( consortium != null )
				if ( consortium.consortiumId != 0 )
			   hasPK = true;
			
			return( hasPK );
	    }
	
		protected string getSubclassName()
		{ return( "ConsortiumController" ); }
		
		override protected ILogger getLogger()
		{ return( logger ); }

	
//************************************************************************    
// Attributes
//************************************************************************
	    private Consortium consortium 			= null;
		private readonly ILogger<ConsortiumController> logger;
	}    
}


