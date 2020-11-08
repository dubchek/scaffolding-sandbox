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
	public class AccountDAO
	{ 
        ///<summary>
        /// default constructor, using dependency injection to acquire an ILogger<AccountDAO> interface
        /// <param name="_logger|></para>
        /// </summary>
		public AccountDAO( ILogger<AccountDAO> _logger )
		{
			logger = _logger;
		}
		
        ///<summary>
        /// Retrieves a Account from the persistent store, using the provided primary key. 
        /// If no match is found, a null Account is returned.
        /// <paramm name="pk"></para>
        /// </summary>

    	public Account findAccount( AccountPrimaryKey pk ) 
    	{
    		Account model = null;
    		
        	if (pk == null)
        	{
            	throw( new ProcessingException("AccountDAO.findAccount(...) cannot have a null primary key argument") );
        	}

			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
		        	try
	    	    	{
	                    model = new Account();
	                    model.copy( session.Get<Account>(pk.getFirstKey()) );
					}
					catch( Exception exc )
					{
						model = null;
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "AccountDAO.findAccount failed for primary key " + pk + " - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}		    
					
        	return( model );
    	}
	    
        ///<summary>
        /// returns a List of all the Account entities
        /// <returns></returns>
        ///</summary>
	    public List<Account> findAllAccount()
	    {
			List<Account> refList = new List<Account>();
			IList list;

     		using (ISession session = FrameworkPersistenceHelper.OpenSession())
     		{
	            using (ITransaction transaction = session.BeginTransaction())
    	        {
					try
					{
						string buf 	= "from Account";
				 		IQuery query = session.CreateQuery( buf );
				 		
						if ( query != null )
						{
							list = query.List();
							Account model = null;
							
			                foreach (Account listEntry in list)
			                {
			                    model = new Account();
			                    model.copyShallow(listEntry);
			                    refList.Add(model);
			                }
						}
					}
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc);
						throw ( new ProcessingException( "AccountDAO.findAllAccount failed - " + exc.ToString() ) );		
					}		
					finally
					{
					}
				}
			}
			if ( refList.Count <= 0 )
			{
				logger.LogInformation( "AccountDAO:findAllAccounts() - List is empty.");
			}
	        
			return( refList );		        
	    }
		
        ///<summary>
        /// Inserts a new Account model into the persistent store and returns 
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Account createAccount( Account model )
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
							string errMsg = "AccountDAO.createAccount - null model provided but not allowed";
							logger.LogInformation( errMsg );
							throw ( new ProcessingException( errMsg ) );		
						}		        	    
					}
					catch( Exception exc )
					{
						string errMsg = "AccountDAO.createAccount - Hibernate failed to rollback - " + exc.ToString();
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
        /// Updates the provided Account model to the persistent store.
        /// <param name="model"></para>
        /// <returns></returns>
        ///</summary>
	    public Account saveAccount( Account model )
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
						string errMsg = "AccountDAO.saveAccount - Hibernate failed to rollback - " + exc.ToString();
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
        /// Removes the associated Account model from the persistent store.
        /// <param name="pk"></para>
        /// <returns></returns>
        ///</summary>
	    public bool deleteAccount( AccountPrimaryKey pk ) 
	    {
	    	bool deleted = false;
	    	
			using (ISession session = FrameworkPersistenceHelper.OpenSession())
			{
                using (ITransaction transaction = session.BeginTransaction())
                {
			    	try
	    			{
	    				Account model = findAccount(pk);    	
	    			
	    				session.Delete( model );
                    	transaction.Commit();
                    	deleted = true;
                	}	    
					catch( Exception exc )
					{
						Console.WriteLine("Exception caught: {0}", exc );
						logger.LogInformation( "AccountDAO.deleteAccount failed - " + exc.ToString() );
						throw ( new ProcessingException( "AccountDAO.deleteAccount failed - " + exc.ToString() ) );					
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
		private readonly ILogger<AccountDAO> logger;
	}

}


