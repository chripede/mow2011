using System;
using System.Web;
using NHibernate;

namespace ContactListWeb.Infrastructure.NHibernate
{
    public class NHibernateSessionModule : IHttpModule
    {
        private HttpApplication app;

        public void Init(HttpApplication context)
        {
            app = context;

            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
            context.Error += Error;
        }

        private void Error(object sender, EventArgs e)
        {
            var localFactory = SessionManager.Instance.SessionFactory;
            var session = LazySessionContext.UnBind(localFactory);
            if(session != null)
            {
                if(session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                }
                session.Dispose();
            }
        }

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            var localFactory = SessionManager.Instance.SessionFactory;
            LazySessionContext.Bind(new Lazy<ISession>(() => BeginSession(localFactory)), localFactory);
        }

        private static ISession BeginSession(ISessionFactory sf)
        {
            var session = sf.OpenSession();
            session.BeginTransaction();
            return session;
        }

        private void ContextEndRequest(object sender, EventArgs e)
        {
            var localFactory = SessionManager.Instance.SessionFactory;
            var session = LazySessionContext.UnBind(localFactory);
            if (session != null)
                EndSession(session);
        }

        private static void EndSession(ISession session)
        {
            if(session.Transaction != null && session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
            session.Dispose();
        }


        public void Dispose()
        {
            app.BeginRequest -= ContextBeginRequest;
            app.EndRequest -= ContextEndRequest;
        }
    }
}