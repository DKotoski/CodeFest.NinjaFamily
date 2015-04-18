using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using CodeFest.NinjaFamily.FamilyTreeApp.Models;

namespace CodeFest.NinjaFamily.FamilyTreeApp.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using CodeFest.NinjaFamily.FamilyTreeApp.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Relationship>("Relationship");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class RelationshipController : ODataController
    {
        private UsersContext db = new UsersContext();

        //this should create the rellationship
        public IHttpActionResult CreateRelationship(int invokerID, User created)
        {
            var asParent = db.Relationships.FirstOrDefault(x => x.Parrent == invokerID);
            var asChild = db.Relationships.FirstOrDefault(x => x.Child == invokerID);
            if (asParent != null || asChild!=null)
            {
                return BadRequest();
            }
            else if (asParent == null)
            {
                db.Users.Add(created);
                Relationship rel = new Relationship { Parrent = invokerID, Child = created.ID };
                db.Relationships.Add(rel);
                db.SaveChanges();
                //email handler sent mail to created
                return Ok();
            }
            else if (asChild == null)
            {
                db.Users.Add(created);
                Relationship rel = new Relationship { Child = invokerID, Parrent = created.ID };
                db.Relationships.Add(rel);
                db.SaveChanges();
                //email handler sent mail to created
                return Ok();
            }
                return BadRequest();
        }
        // GET odata/Relationship
        [Queryable]
        public IQueryable<Relationship> GetRelationship()
        {
            return db.Relationships;
        }

        // GET odata/Relationship(5)
        [Queryable]
        public SingleResult<Relationship> GetRelationship([FromODataUri] int key)
        {
            return SingleResult.Create(db.Relationships.Where(relationship => relationship.ID == key));
        }

        // PUT odata/Relationship(5)
        public IHttpActionResult Put([FromODataUri] int key, Relationship relationship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != relationship.ID)
            {
                return BadRequest();
            }

            db.Entry(relationship).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationshipExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(relationship);
        }

        // POST odata/Relationship
        public IHttpActionResult Post(Relationship relationship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Relationships.Add(relationship);
            db.SaveChanges();

            return Created(relationship);
        }

        // PATCH odata/Relationship(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Relationship> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Relationship relationship = db.Relationships.Find(key);
            if (relationship == null)
            {
                return NotFound();
            }

            patch.Patch(relationship);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RelationshipExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(relationship);
        }

        // DELETE odata/Relationship(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Relationship relationship = db.Relationships.Find(key);
            if (relationship == null)
            {
                return NotFound();
            }

            db.Relationships.Remove(relationship);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RelationshipExists(int key)
        {
            return db.Relationships.Count(e => e.ID == key) > 0;
        }
    }
}
