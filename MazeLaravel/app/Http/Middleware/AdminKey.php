<?php

namespace App\Http\Middleware;

use Closure;
use App\User;

class AdminKey
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
        
        $token = $request->header('ADMINKEY');
        $adminKey = User::where('status', 'admin')
                            ->where('api_token', $token)
                            ->pluck('id')
                            ->first();

        if($adminKey == null){
            return response()->json(['message' => 'Wrong ADMINKEY'], 401);
        } 
        return $next($request);
    }
}
