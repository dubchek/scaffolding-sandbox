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
	public class ConsortiumDAO
	{ 
        ///<summary>
        /// default constructor, using dependency injection to acquire an ILogger<ConsortiumDAO> interface
        /// <param name="_logger|></para>
        /// </summary>
		public ConsortiumDAO( ILogger<ConsortiumDAO> _logger )
		{
			logger = _logger;
		}
		
        ///<summary>
        /// Retrieves a Consortium from the persistent store, using the provided primary key. 
        /// If no match is found, a null Consortium is returned.
        /// <paramm name="pk"></para>
        /// </summary>

    	public Consortium findConsortium( ConsortiumPrimaryKey pk ) 
    	{
    		Consortium model = null;
    		
        	if (pk == null)
        	{
            	throw( new ProcessingException("ConsortiumDAO.findConsortium(...) cannot have a null primary key argument") );
        	}

			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
		        	try
	    	    	{
	                    model = new Consortium();
	                    model.copy( session.Get<Consortium>(pk.getFirstKey()) );
					}
					catch( Exception exc )
					{
						model = null;
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "ConsortiumDAO.findConsortium failed for primary key " + pk + " - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}		    
					
        	return( model );
    	}
	    
        ///<summary>
        /// returns a List of all the Consortium entities
        /// <returns></returns>
        ///</summary>
	    public List<Consortium> findAllConsortium()
	    {
			List<Consortium> refList = new List<Consortium>();
			IList list;

     		using (ISession session = FrameworkPersistenceHelper.OpenSession())
     		{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
					try
					{
						string buf 	= "from Consortium";
				 		IQuery query = session.CreateQuery( buf );
				 		
						if ( query != null )
						{
							list = query.List();
							Consortium model = null;
							
			                foreach (Consortium listEntry in list)
			                {
			                    model = new Consortium();
			                    model.copyShallow(listEntry);
			                    refList.Add(model);
			                }
						}
					}
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "ConsortiumDAO.findAllConsortium failed - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}
			if ( refList.Count <= 0 )
			{
				logger.LogInformation( "ConsortiumDAO:findAllConsortiums() - List is empty.");
			}
	        
			return( refList );		        
	    }
		
        ///<summary>
        /// Inserts a new Consortium model into the persistent store and returns 
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Consortium createConsortium( Consortium model )
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
							string errMsg = "ConsortiumDAO.createConsortium - null model provided but not allowed";
							logger.LogInformation( errMsg );
							throw ( new ProcessingException( errMsg ) );		
						}		        	    
					}
					catch( Exception exc )
					{
						string errMsg = "ConsortiumDAO.createConsortium - Hibernate failed to rollback - " + exc.ToString();
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
        /// Updates the provided Consortium model to the persistent store.
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Consortium saveConsortium( Consortium model )
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
						string errMsg = "ConsortiumDAO.saveConsortium - Hibernate failed to rollback - " + exc.ToString();
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
        /// Removes the associated Consortium model from the persistent store.
        /// <param name="pk"></para>
        /// <returns></returns>
        ///</summary>
	    public bool deleteConsortium( ConsortiumPrimaryKey pk ) 
	    {
	    	bool deleted = false;
	    	
			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
                using (ITransaction transaction = session.BeginTransaction())
                {
			    	try
	    			{
	    				Consortium model = findConsortium(pk);    	
	    			
	    				session.Delete( model );
                    	transaction.Commit();
                    	deleted = true;
                	}	    
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc );
						logger.LogInformation( "ConsortiumDAO.deleteConsortium failed - " + exc.ToString() );
						throw ( new ProcessingException( "ConsortiumDAO.deleteConsortium failed - " + exc.ToString() ) );					
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
		private readonly ILogger<ConsortiumDAO> logger;
	}

}


