using System;
using System.Collections.Generic;

using ScEngineNet.SafeElements;
using ScEngineNet.NativeElements;

namespace ScEngineNet
{
   public class ScMemoryContext
    {
        private IntPtr scMemoryContext;

        #region initialize

        public bool IsValid()
        {
            bool isValid = scMemoryContext != IntPtr.Zero;
            return isValid;
        }

        public static bool IsMemoryInitialized()
        {
            return NativeMethods.sc_memory_is_initialized();
        }

        internal ScMemoryContext(IntPtr context)
        {
            this.scMemoryContext = context;
        }

        public ScMemoryContext(ScAccessLevels accessLevels)
        {
            if (ScMemoryContext.IsMemoryInitialized())
            {
                this.scMemoryContext = NativeMethods.sc_memory_context_new((byte)accessLevels);
            }
        }

        public void Delete()
        {
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_memory_context_free(this.scMemoryContext);
                this.scMemoryContext = IntPtr.Zero;
            }
        }

        public ScStat GetStatistics()
        {
            var stat = new ScStat();
            if (ScMemoryContext.IsMemoryInitialized())
            {
                NativeMethods.sc_memory_stat(this.scMemoryContext, out stat);
            }
            return stat;
        }

        public bool SaveState()
        {
            var result = ScResult.SC_RESULT_ERROR;
            if (ScMemoryContext.IsMemoryInitialized())
            {
                result = NativeMethods.sc_memory_save(this.scMemoryContext);
            }
            return result == ScResult.SC_RESULT_OK;
        }
     
        #endregion

        #region Elements

        #region Common

        public Identifier CreateUniqueIdentifier(ScNode node)
        {
            return Identifier.GetUnique(this.scMemoryContext, node);
        }

        public Identifier CreateUniqueIdentifier(string prefix, ScNode node)
        {
            return Identifier.GetUnique( this.scMemoryContext, prefix, node);
        }

        public bool IsElementExist(ScAddress elementAddress)
        {
            return ScMemorySafeMethods.IsElementExist(this.scMemoryContext, elementAddress);
        }

        public bool DeleteElement(ScElement element)
        {
            return ScMemorySafeMethods.DeleteElement(this.scMemoryContext, element);
        }

        #endregion

        #region Arcs

        public ScArc CreateArc(ScElement beginElement, ScElement endElement, ElementType elementType)
        {
            var createdArc = new ScArc(ScAddress.Invalid, this.scMemoryContext);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdArc = new ScArc(new ScAddress(NativeMethods.sc_memory_arc_new(this.scMemoryContext, elementType, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress)), this.scMemoryContext);
            }
            return createdArc;
        }

        public ScArc FindArc(ScAddress arcAddress)
        {
            return new ScArc(arcAddress, this.scMemoryContext);
        }

        public bool ArcIsExist(ScElement beginElement, ScElement endElement, ElementType arcType)
        {
            bool result = false;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                result = NativeMethods.sc_helper_check_arc( this.scMemoryContext, beginElement.ScAddress.WScAddress, endElement.ScAddress.WScAddress, arcType);
            }
            return result;

        }

        #endregion

        #region Nodes

        public ScNode CreateNode(ElementType elementType)
        {
            var createdNode = new ScNode(ScAddress.Invalid, this.scMemoryContext);
            if (ScMemoryContext.IsMemoryInitialized() == true) 
            {
                createdNode = new ScNode(new ScAddress(NativeMethods.sc_memory_node_new( this.scMemoryContext, elementType)), this.scMemoryContext);
            }
            return createdNode;
        }

        public ScNode CreateNode(ElementType elementType, Identifier identifier)
        {
            ScNode createdNode = this.FindNode(identifier);
            if (createdNode.ScAddress == ScAddress.Invalid)
            {
                createdNode = this.CreateNode(elementType);
                createdNode.Identifier = identifier;
            }
            return createdNode;
        }

        public ScNode FindNode(Identifier identifier)
        {
            return ScMemorySafeMethods.FindNode(this.scMemoryContext, identifier);
        }

        public ScNode FindNode(ScAddress nodeAddress)
        {
            return new ScNode(nodeAddress, this.scMemoryContext);
        }

        #endregion

        #region Links

        public ScLink CreateLink()
        {
            var createdLink = new ScLink(ScAddress.Invalid,  this.scMemoryContext);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                createdLink = new ScLink(new ScAddress(NativeMethods.sc_memory_link_new( this.scMemoryContext)),  this.scMemoryContext);
            }
            return createdLink;
        }

        public ScLink CreateLink(ScLinkContent content)
        {
            List<ScLink> findLinks = this.FindLinks(content);
            if (findLinks.Count>0)
            {
                return findLinks[0];
            }
            ScLink createdLink = this.CreateLink();
            createdLink.LinkContent = content;
            return createdLink;
        }

        public ScLink FindLink(ScAddress linkAddress)
        {
            return new ScLink(linkAddress, this.scMemoryContext);
        }

        public List<ScLink> FindLinks(ScLinkContent content)
        {
            List<ScLink> links = new List<ScLink>();
            IntPtr adressesPtr = IntPtr.Zero;
            uint resulCount = 0;
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {
                NativeMethods.sc_memory_find_links_with_content( this.scMemoryContext, content.ScStream, out adressesPtr, out resulCount);
            }
            Array addressesArray = NativeMethods.PtrToArray(typeof(WScAddress), adressesPtr, resulCount);
            for (uint index = 0; index < resulCount; index++)
            {
                links.Add(new ScLink(new ScAddress((WScAddress)addressesArray.GetValue(index)),  this.scMemoryContext));
            }
            NativeMethods.sc_memory_free_buff(adressesPtr);
            return links;
        }

        #endregion

        #endregion

        #region Events

        public ScEvent CreateEvent(ScElement element, ScEventType eventType)
        {
            var scEvent = new ScEvent(element.ScAddress, eventType);
            if (ScMemoryContext.IsMemoryInitialized() == true)
            {

                scEvent.Subscribe( scMemoryContext);
            }
            return scEvent;
        }

        #endregion

        #region Iterators

        public ScContainer CreateContainer(ScElement e1, ElementType t1, ElementType t2)
        {
            return new ScContainer( this.scMemoryContext, e1, t1, t2);
        }

        public ScContainer CreateContainer(ElementType t1, ElementType t2, ScElement e1)
        {
            return new ScContainer( this.scMemoryContext, t1, t2, e1);
        }

        public ScContainer CreateContainer(ScElement e1, ElementType t1, ScElement e2)
        {
            return new ScContainer(this.scMemoryContext, e1, t1, e2);
        }

        public ScContainer CreateContainer(ElementType t1, ElementType t2, ScElement e1, ElementType t3, ElementType t4)
        {
            return new ScContainer( this.scMemoryContext, t1, t2, e1, t3, t4);
        }

        public ScContainer CreateContainer(ElementType t1, ElementType t2, ScElement e1, ElementType t3, ScElement e2)
        {
            return new ScContainer( this.scMemoryContext, t1, t2, e1, t3, e2);
        }

        public ScContainer CreateContainer(ScElement e1, ElementType t1, ElementType t2, ElementType t3, ElementType t4)
        {
            return new ScContainer( this.scMemoryContext, e1, t1, t2, t3, t4);
        }

        public ScContainer CreateContainer(ScElement e1, ElementType t1, ElementType t2, ElementType t3, ScElement e2)
        {
            return new ScContainer( this.scMemoryContext, e1, t1, t2, t3, e2);
        }

        public ScContainer CreateContainer(ScElement e1, ElementType t1, ScElement e2, ElementType t2, ElementType t3)
        {
            return new ScContainer( this.scMemoryContext, e1, t1, e2, t2, t3);
        }

        public ScContainer CreateContainer(ScElement e1, ElementType t1, ScElement e2, ElementType t2, ScElement e3)
        {
            return new ScContainer( this.scMemoryContext, e1, t1, e2, t2, e3);
        }

        #endregion

        #region Члены IDisposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                //unmanaged
                this.Delete();
                this.scMemoryContext = IntPtr.Zero;
            }
        }

        ~ScMemoryContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
