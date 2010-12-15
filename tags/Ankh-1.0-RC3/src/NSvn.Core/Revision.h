// $Id$
#using <mscorlib.dll>
#include "svnenums.h"
#include "Pool.h"
#include "utils.h"

namespace NSvn
{
    namespace Core
    {
        using namespace System;

        [Serializable]
        public __gc class Revision 
        {
        public:
            /// <summary> No revision information given.</summary>    
            static Revision* const Unspecified = new Revision( svn_opt_revision_unspecified );
            /// <summary>Revision of item's last commit in working copy.</summary>        
            static Revision* const Committed = new Revision( svn_opt_revision_committed );
            /// <summary>Revision before item's last commit in working copy. 
            ///						  (current committed revision in wc) - 1</summary>      
            static Revision* const Previous = new Revision( svn_opt_revision_previous );
            /// <summary>Base revision of item's working copy.</summary>        
            static Revision* const Base = new Revision( svn_opt_revision_base );
            /// <summary>Current committed revision, plus local changes in working copy.</summary>           
            static Revision* const Working = new Revision( svn_opt_revision_working );
            /// <summary>Latest revision in repository.</summary> 
            static Revision* const Head = new Revision( svn_opt_revision_head );

            /// <summary>Creates a revision from a revision number</summary>
            static Revision* FromNumber( int revision );

            /// <summary>Creates a revision from a date</summary>
            static Revision* FromDate( DateTime date );

            /// <summary>Create a revision by parsing a string.</summary>
            static Revision* Parse( String* string );

            // convert to an svn_opt_revision_t*
            // allocate in pool
            svn_opt_revision_t* ToSvnOptRevision( const Pool& pool );

            String* ToString();

            /// <summary>The revision as a number. Can only be used if .Type is Number.</summary>
            __property int get_Number()
            {
                if ( this->Type != RevisionType::Number )
                    throw new InvalidOperationException( S"Revision type is not Number" );
                return this->revision;
            }

            /// <summary>The revision as a date. Can only be used if .Type is Date.</summary>
            __property DateTime get_Date()
            {
                if ( this->Type != RevisionType::Date )
                    throw new InvalidOperationException( S"Revision type is not Date" );
                return this->date;
            }

            /// <summary>The type of the revision.</summary>
            __property RevisionType get_Type()
            {
                return static_cast<RevisionType>(kind);
            }

        private:
            Revision( svn_opt_revision_kind kind ) : kind( kind )
            {;}
            DateTime date;       
            int revision;
            svn_opt_revision_kind kind;

        };
    }
}
