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
	public class ClientDAO
	{ 
        ///<summary>
        /// default constructor, using dependency injection to acquire an ILogger<ClientDAO> interface
        /// <param name="_logger|></para>
        /// </summary>
		public ClientDAO( ILogger<ClientDAO> _logger )
		{
			logger = _logger;
		}
		
        ///<summary>
        /// Retrieves a Client from the persistent store, using the provided primary key. 
        /// If no match is found, a null Client is returned.
        /// <paramm name="pk"></para>
        /// </summary>

    	public Client findClient( ClientPrimaryKey pk ) 
    	{
    		Client model = null;
    		
        	if (pk == null)
        	{
            	throw( new ProcessingException("ClientDAO.findClient(...) cannot have a null primary key argument") );
        	}

			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
		        	try
	    	    	{
	                    model = new Client();
	                    model.copy( session.Get<Client>(pk.getFirstKey()) );
					}
					catch( Exception exc )
					{
						model = null;
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "ClientDAO.findClient failed for primary key " + pk + " - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}		    
					
        	return( model );
    	}
	    
        ///<summary>
        /// returns a List of all the Client entities
        /// <returns></returns>
        ///</summary>
	    public List<Client> findAllClient()
	    {
			List<Client> refList = new List<Client>();
			IList list;

     		using (ISession session = FrameworkPersistenceHelper.OpenSession())
     		{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
					try
					{
						string buf 	= "from Client";
				 		IQuery query = session.CreateQuery( buf );
				 		
						if ( query != null )
						{
							list = query.List();
							Client model = null;
							
			                foreach (Client listEntry in list)
			                {
			                    model = new Client();
			                    model.copyShallow(listEntry);
			                    refList.Add(model);
			                }
						}
					}
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "ClientDAO.findAllClient failed - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}
			if ( refList.Count <= 0 )
			{
				logger.LogInformation( "ClientDAO:findAllClients() - List is empty.");
			}
	        
			return( refList );		        
	    }
		
        ///<summary>
        /// Inserts a new Client model into the persistent store and returns 
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Client createClient( Client model )
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
							string errMsg = "ClientDAO.createClient - null model provided but not allowed";
							logger.LogInformation( errMsg );
							throw ( new ProcessingException( errMsg ) );		
						}		        	    
					}
					catch( Exception exc )
					{
						string errMsg = "ClientDAO.createClient - Hibernate failed to rollback - " + exc.ToString();
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
        /// Updates the provided Client model to the persistent store.
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Client saveClient( Client model )
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
						string errMsg = "ClientDAO.saveClient - Hibernate failed to rollback - " + exc.ToString();
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
        /// Removes the associated Client model from the persistent store.
        /// <param name="pk"></para>
        /// <returns></returns>
        ///</summary>
	    public bool deleteClient( ClientPrimaryKey pk ) 
	    {
	    	bool deleted = false;
	    	
			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
                using (ITransaction transaction = session.BeginTransaction())
                {
			    	try
	    			{
	    				Client model = findClient(pk);    	
	    			
	    				session.Delete( model );
                    	transaction.Commit();
                    	deleted = true;
                	}	    
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc );
						logger.LogInformation( "ClientDAO.deleteClient failed - " + exc.ToString() );
						throw ( new ProcessingException( "ClientDAO.deleteClient failed - " + exc.ToString() ) );					
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
		private readonly ILogger<ClientDAO> logger;
	}

}


