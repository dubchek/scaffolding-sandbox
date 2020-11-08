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
using NHibernate;
using NHibernate.Cfg;

using demo.Exceptions;
using demo.Models;
using demo.PrimaryKeys;
using demo.Persistence;

namespace demo.DAOs
{
    /// <summary>
    /// Responsible for interacting with the ORM abstraction to create, read, update, 
    /// and delete an demo entity along with its associated entities
    /// </summary>
	public class ConverterDAO
	{ 
        ///<summary>
        /// default constructor, using dependency injection to acquire an ILogger<ConverterDAO> interface
        /// <param name="_logger|></para>
        /// </summary>
		public ConverterDAO( ILogger<ConverterDAO> _logger )
		{
			logger = _logger;
		}
		
        ///<summary>
        /// Retrieves a Converter from the persistent store, using the provided primary key. 
        /// If no match is found, a null Converter is returned.
        /// <paramm name="pk"></para>
        /// </summary>

    	public Converter findConverter( ConverterPrimaryKey pk ) 
    	{
    		Converter model = null;
    		
        	if (pk == null)
        	{
            	throw( new ProcessingException("ConverterDAO.findConverter(...) cannot have a null primary key argument") );
        	}

			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
		        	try
	    	    	{
	                    model = new Converter();
	                    model.copy( session.Get<Converter>(pk.getFirstKey()) );
					}
					catch( Exception exc )
					{
						model = null;
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "ConverterDAO.findConverter failed for primary key " + pk + " - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}		    
					
        	return( model );
    	}
	    
        ///<summary>
        /// returns a List of all the Converter entities
        /// <returns></returns>
        ///</summary>
	    public List<Converter> findAllConverter()
	    {
			List<Converter> refList = new List<Converter>();
			IList list;

     		using (ISession session = FrameworkPersistenceHelper.OpenSession())
     		{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
					try
					{
						string buf 	= "from Converter";
				 		IQuery query = session.CreateQuery( buf );
				 		
						if ( query != null )
						{
							list = query.List();
							Converter model = null;
							
			                foreach (Converter listEntry in list)
			                {
			                    model = new Converter();
			                    model.copyShallow(listEntry);
			                    refList.Add(model);
			                }
						}
					}
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "ConverterDAO.findAllConverter failed - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}
			if ( refList.Count <= 0 )
			{
				logger.LogInformation( "ConverterDAO:findAllConverters() - List is empty.");
			}
	        
			return( refList );		        
	    }
		
        ///<summary>
        /// Inserts a new Converter model into the persistent store and returns 
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Converter createConverter( Converter model )
	    {
     		using (ISession session = FrameworkPersistenceHelper.OpenSession())
     		{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
			    	try
			    	{
			    		if ( model != null )
			    		{
		    	        	session.Save(model);
		        	    	transaction.Commit();
		        	    }
		        	    else
						{
							string errMsg = "ConverterDAO.createConverter - null model provided but not allowed";
							logger.LogInformation( errMsg );
							throw ( new ProcessingException( errMsg ) );		
						}		        	    
					}
					catch( Exception exc )
					{
						string errMsg = "ConverterDAO.createConverter - Hibernate failed to rollback - " + exc.ToString();
						Console.WriteLine("Exception caught: {0}", exc.ToString());
						logger.LogInformation( errMsg );
						throw ( new ProcessingException( errMsg ) );		
					}		
					finally
					{
					}		    
			        return( model );
				}
			}	 
	    }
		    
        ///<summary>
        /// Updates the provided Converter model to the persistent store.
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Converter saveConverter( Converter model )
	    {
     		using (ISession session = FrameworkPersistenceHelper.OpenSession())
     		{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
			    	try
			    	{
		    	        session.Update(model);
		        	    transaction.Commit();
					}
					catch( Exception exc )
					{
						string errMsg = "ConverterDAO.saveConverter - Hibernate failed to rollback - " + exc.ToString();
						Console.WriteLine("Exception caught: {0}", exc.ToString());
						logger.LogInformation( errMsg );
						throw ( new ProcessingException( errMsg ) );		
					}		
					finally
					{
					}		    
			        return( model );
				}
			}	 
	    }
	    
        ///<summary>
        /// Removes the associated Converter model from the persistent store.
        /// <param name="pk"></para>
        /// <returns></returns>
        ///</summary>
	    public bool deleteConverter( ConverterPrimaryKey pk ) 
	    {
	    	bool deleted = false;
	    	
			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
                using (ITransaction transaction = session.BeginTransaction())
                {
			    	try
	    			{
	    				Converter model = findConverter(pk);    	
	    			
	    				session.Delete( model );
                    	transaction.Commit();
                    	deleted = true;
                	}	    
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc );
						logger.LogInformation( "ConverterDAO.deleteConverter failed - " + exc.ToString() );
						throw ( new ProcessingException( "ConverterDAO.deleteConverter failed - " + exc.ToString() ) );					
					}		
					finally
					{
					}
				}	    			    	
			}
			
			return deleted;
	    }

 
//*****************************************************
// Attributes
//*****************************************************
		private readonly ILogger<ConverterDAO> logger;
	}

}


