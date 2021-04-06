<?php

namespace App\Http\Middleware;

use Closure;
use App\User;

class UserKey
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    {
        $tokenUser = $request->header('USERKEY');
        $tokenAdmin = $request->header('ADMINKEY');
        $token = array($tokenUser, $tokenAdmin);


        $usersKey = User::whereIn('api_token', $token)->pluck('id')->first();


        if(array($usersKey) == [null]){
            return response()->json(['message' => 'Wrong USERKEY or ADMINKEY'], 401);
        } 
        return $next($request);
    }
}
