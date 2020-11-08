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
 * Implements Struts action processing for model Converter.
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
	public class ConverterController : BaseController
	{
		/**
		 * default constructor, using dependency injection to acquire a
		 * ILogger implementation
		 */
		public ConverterController( ILogger<ConverterController> _logger )
		{
			logger = _logger;
		}

		/**
		 * redirect to the profile .cshtml 
		 */	
	    public IActionResult Profile( string converterId, string action, string parentUrl )
        {
            ViewData["converterId"] = converterId;
            ViewData["action"]			= action;
            ViewData["parentUrl"] 		= parentUrl;
        
            return PartialView("~/Views/ConverterProfile.cshtml");
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

            return PartialView("~/Views/ConverterList.cshtml");
        }
	
	    /**
	     * handles saving a Converter BO.  if no key provided, calls create, otherwise calls save
	     */
	    public JsonResult save( [FromBody] Converter model )
	    {
			converter = model;
			
			logger.LogInformation( "Converter.save() on - " + model.ToString()  );
			
	        if ( hasPrimaryKey() )
	        {
	            converter = update();
	        }
	        else
	        {
	            converter = create();
	        }
	        
	        return Json(converter);
	    }
	
	    /**
	     * handles updating a Converter model
	     */    
	    protected Converter update()
	    {
	    	// store provided data
	        Converter model = converter;
	
	        // load actual data from storage
	        loadHelper( converter.getConverterPrimaryKey() );
	    	
	    	// copy provided data into actual data
	    	converter.copyShallow( model );
	    	
	        try
	        {                        	        
	            this.converter = ConverterDelegate.getConverterInstance().saveConverter( converter );
	            
	            if ( this.converter != null )
	            	logger.LogInformation( "ConverterController:update() - successfully updated Converter - " + converter.ToString() );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "ConverterController:update() - successfully update Converter - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.converter;
	        
	    }
	
	    /**
	     * handles creating a Converter model
	     */
	    protected Converter create()
	    {
	        try
	        {       
				this.converter 	= ConverterDelegate.getConverterInstance().createConverter( converter );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	
	        	string errMsg = "ConverterController:create() - exception Converter - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return this.converter;
	    }
	
	    /**
	     * handles deleting a Converter model
	     */
	    public JsonResult delete( String converterId, String[] childIds ) 
	    {                
	        try
	        {
	        	if ( childIds == null || childIds.Length == 0 )
	        	{
	        		long parentId  =  convertToLong( converterId );
	        		ConverterDelegate.getConverterInstance().delete( new ConverterPrimaryKey( parentId  ) );
	        		logger.LogInformation( "ConverterController:delete() - successfully deleted Converter with key " + converter.getConverterPrimaryKey().keys().ToString());
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
	        					ConverterDelegate.getConverterInstance().delete( new ConverterPrimaryKey( tmpId ) );
	        			}
		                catch( Exception exc )
		                {
		                	signalBadRequest();
	
		                	string errMsg = "ConverterController:delete() - " + exc.ToString();
		                	logger.LogError( errMsg );
		                	//throw ( new ProcessingException( errMsg ) );
		                }
	        		}
	        	}
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	        	string errMsg = "ConverterController:delete() - " + exc.ToString();
	        	logger.LogError( errMsg );
	        	//throw ( new ProcessingException( errMsg ) );
	        }
	        
	        return null;
		}        
		
	    /**
	     * handles loading a Converter model
	     */    
	    public JsonResult load( String converterId ) 
	    {    	
	        ConverterPrimaryKey pk 	= null;
			long id 					= convertToLong( converterId );
			
	    	try
	        {
	    		logger.LogInformation( "Converter.load pk is " + id );
	    		
	        	if ( id != 0 )
	        	{
	        		pk = new ConverterPrimaryKey( id );
	
	        		loadHelper( pk );
			            
		            logger.LogInformation( "ConverterController:load() - successfully loaded - " + this.converter.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "ConverterController:load() - unable to locate the primary key as an attribute or a selection for - " + converter.ToString();				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ConverterController:load() - failed to load Converter using Id " + id + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	       return Json(converter);
	    }
	
	    /**
	     * handles loading all Converter models
	     */
	    public JsonResult loadAll()
	    {                
	        List<Converter> converterList = null;
	        
	    	try
	        {                        
	            // load the Converter
	            converterList = ConverterDelegate.getConverterInstance().getAllConverter();
	            
	            if ( converterList != null && converterList.Count > 0 )
	            	logger.LogInformation(  "ConverterController:loadAllConverter() - successfully loaded all Converters" );
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ConverterController:loadAll() - failed to load all Converters - " + exc.ToString();				
				logger.LogError( errMsg );
	            throw new ProcessingException( errMsg );            
	        }

	       	return Json(converterList);
	                            
	    }
	
// findAllBy methods
 

	    protected Converter loadHelper( ConverterPrimaryKey pk )
	    {
	    	try
	        {
	    		logger.LogInformation( "Converter.loadHelper primary key is " + pk);
	    		
	        	if ( pk != null )
	        	{
	        		// load the contained instance of Converter
		            this.converter = ConverterDelegate.getConverterInstance().getConverter( pk );
		            
		            logger.LogInformation( "ConverterController:loadHelper() - successfully loaded - " + this.converter.ToString() );             
				}
				else
				{
		        	signalBadRequest();
	
					string errMsg = "ConverterController:loadHelper() - null primary key provided.";				
					logger.LogError( errMsg );
		            throw ( new ProcessingException( errMsg ) );
				}	            
	        }
	        catch( Exception exc )
	        {
	        	signalBadRequest();
	
	        	string errMsg = "ConverterController:load() - failed to load Converter using pk " + pk + ", " + exc.ToString();				
				logger.LogError( errMsg );
	            throw ( new ProcessingException( errMsg ) );
	        }
	
	        return converter;
	
	    }
	

	    /**
	     * returns true if the converter is non-null and has it's primary key field(s) set
	     */
	    protected bool hasPrimaryKey()
	    {
	    	bool hasPK = false;
	
			if ( converter != null )
				if ( converter.converterId != 0 )
			   hasPK = true;
			
			return( hasPK );
	    }
	
		protected string getSubclassName()
		{ return( "ConverterController" ); }
		
		override protected ILogger getLogger()
		{ return( logger ); }

	
//************************************************************************    
// Attributes
//************************************************************************
	    private Converter converter 			= null;
		private readonly ILogger<ConverterController> logger;
	}    
}


