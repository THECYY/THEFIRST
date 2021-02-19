using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using MysqlManager.Entity;

namespace MysqlManager.NHibernateManager
{
    class CardManager
    {
        public static void Save(Card card)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(card);
                    transaction.Commit();
                }
            }
        }

        public static void SaveAll(List<Card> cards)
        {
            foreach(Card c in cards){
                Save(c);
            }
        }

        public static void Update(Card card)
        {

            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (CardManager.getById(card.Id) != null)
                    {
                        session.Update(card);
                    }
                    transaction.Commit();
                }
            }
        }

        public static void Delete(Card card)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (CardManager.getById(card.Id) != null)
                    {
                        session.Delete(card);
                    }
                    transaction.Commit();
                }
            }
        }

        public static Card getById(int id)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Card card = session.Get<Card> (id);
                    transaction.Commit();
                    return card;
                }
            }
        }


        public static IList<Card> getAllCard()
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(Card));
                    IList<Card> cards = criteria.List<Card> ();
                    transaction.Commit();
                    return cards;
                }
            }
        }

        public static IList<Card> getByName(String name)
        {
            using (ISession session = NHibernateHelper.getFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ICriteria criteria = session.CreateCriteria(typeof(Card)).Add(Restrictions.Eq("Name", name));
                    IList<Card> p = criteria.List<Card>();
                    transaction.Commit();
                    return p;
                }
            }
        }


    }
}
