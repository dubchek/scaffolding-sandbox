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

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

using demo.Exceptions;
using System.IO;

namespace demo.Persistence
{
    /// <summary>
    /// Start up methodology for getting nHibernate started automatically via a static constructor
    /// </summary>
    public class FrameworkPersistenceHelper
    {
        /// <summary>
        /// factory singleton pattern of the nHibernate Session Factory interface
        /// </summary>
        private static ISessionFactory _sessionFactory;

        /// <summary>
        /// get method for the Session Factory interface singleton
        /// <return></return>
        /// </summary>
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    try
                    {
                        var configuration = new Configuration();
                        //configuration.Configure();
                        configuration.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml"));
                        configuration.AddFile("Account.hbm.xml");
                        configuration.AddFile("Bank.hbm.xml");
                        configuration.AddFile("Client.hbm.xml");
                        configuration.AddFile("Consortium.hbm.xml");
                        configuration.AddFile("Converter.hbm.xml");
						new SchemaUpdate(configuration).Execute(false, true);
                        //new SchemaExport(configuration).Execute(true, true, false);

                        _sessionFactory = configuration.BuildSessionFactory();
                    }
                    catch( Exception exc )
                    {
                        Console.WriteLine("FrameworkPersistenceHelper.SessionFactory() " + exc.ToString());
                    }
                }
                return _sessionFactory;
            }
        }

        /// <summary>
        /// open/start the hibernate session using the Session Factory singleton interface
        /// <return></return>
        /// </summary>
        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

    }
}