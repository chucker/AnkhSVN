#include <svn_auth.h>
#include <apr_hash.h>
#include <apr_tables.h>
#include ".\authenticationtest.h"
#include "../NSvn.Core/Authentication.h"
#include "../NSvn.Core/AuthenticationProviderObject.h"
#include "../NSvn.Core/credentials.h"
#include "../NSvn.Core/Pool.h"
#include "../NSvn.Core/SvnClientException.h"
#include "../NSvn.Core/StringHelper.h"


#using <mscorlib.dll>
#using <NUnit.Framework.dll>

using namespace System;
using namespace NUnit::Framework;
using namespace NSvn::Core;

namespace
{
    svn_auth_baton_t* GetBaton( svn_auth_provider_object_t* obj, apr_pool_t* pool )
    {
        apr_array_header_t* providers = apr_array_make(pool, 1, sizeof(svn_auth_provider_object_t*) );
        APR_ARRAY_PUSH( providers, svn_auth_provider_object_t* ) =  obj;

        svn_auth_baton_t* authBaton;
        svn_auth_open( &authBaton, providers, pool );

        return authBaton;
    }
}

struct svn_auth_iterstate_t
{};

void NSvn::Core::Tests::MCpp::AuthenticationTest::TestGetSimplePromptProvider()
{
    AuthenticationProviderObject* obj = Authentication::GetSimplePromptProvider( 
        new SimplePromptDelegate(this, SimplePrompt), 3);

    Pool pool;

    svn_auth_cred_simple_t* creds;
    svn_auth_iterstate_t* iterstate;
    apr_hash_t* params = apr_hash_make( pool );

    svn_auth_baton_t* authBaton = GetBaton( obj->GetProvider(), pool );

    HandleError( svn_auth_first_credentials( ((void**)&creds), &iterstate, SVN_AUTH_CRED_SIMPLE, 
        "Realm", authBaton, pool ) );

    Assertion::AssertEquals( "Username not correct", S"Arild", StringHelper( creds->username ) );
    Assertion::AssertEquals( "Password not correct", S"Fines", StringHelper( creds->password ) );


    obj = Authentication::GetSimplePromptProvider( 
        new SimplePromptDelegate(this, NullSimplePrompt), 3);
    authBaton = GetBaton( obj->GetProvider(), pool );

    HandleError( svn_auth_first_credentials( ((void**)&creds), &iterstate, SVN_AUTH_CRED_SIMPLE,
        0, authBaton, pool ) );

    Assertion::Assert( "Creds should be null", creds==0 );
}

void NSvn::Core::Tests::MCpp::AuthenticationTest::TestGetUsernameProvider()
{
    // first get the username through the managed interface
    AuthenticationProviderObject* obj = Authentication::GetUsernameProvider();

    Pool pool;

    svn_auth_cred_username_t* credsManaged;
    svn_auth_iterstate_t* iterstate;
    apr_hash_t* params = apr_hash_make( pool );

    svn_auth_baton_t* authBaton = GetBaton( obj->GetProvider(), pool );

    HandleError( svn_auth_first_credentials( ((void**)&credsManaged), &iterstate, SVN_AUTH_CRED_USERNAME, 
        "Realm", authBaton, pool ) );

       
    // now get it directly
    svn_auth_provider_object_t* authProvider;
    svn_auth_cred_username_t* credsUnmanaged;
    svn_client_get_username_provider( &authProvider, pool );

    authBaton = GetBaton( authProvider, pool );

    HandleError( svn_auth_first_credentials( ((void**)&credsUnmanaged), &iterstate, SVN_AUTH_CRED_USERNAME, 
        "Realm", authBaton, pool ) );

    if ( credsUnmanaged && credsUnmanaged )
        Assertion::AssertEquals( "Usernames different", StringHelper(credsUnmanaged->username), 
            StringHelper(credsManaged->username) );  
    else if ( (credsManaged != 0) ^ (credsUnmanaged != 0) )
        Assertion::Fail( "Credentials are different" );

}
SimpleCredential* NSvn::Core::Tests::MCpp::AuthenticationTest::SimplePrompt( 
    String* realm, String* username )
{
    Assertion::AssertEquals( "Realm string is wrong", S"Realm", realm );
    Assertion::AssertEquals( "Username is wrong", Environment::UserName, username );

    return new SimpleCredential( S"Arild", S"Fines" );
}

SimpleCredential* NSvn::Core::Tests::MCpp::AuthenticationTest::NullSimplePrompt( 
    String* realm, String* username )
{
    Assertion::AssertNull( "Realm should be null", realm );

    return 0;
}