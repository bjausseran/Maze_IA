<?php

namespace App\Http\Controllers;
use App\User;
use Illuminate\Support\Str;
use Illuminate\Http\Request;

class UserController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $users = User::paginate();
        return $users;
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {   
        $inputs = $request->except('_token', 'status', 'api_token');
        $user = new User();
        foreach($inputs as $key => $value) 
        {
            $user->$key = $value;
        }
        $user->status = 'user';
        $user->api_token = Str::random(60);
        $user->save();
        $user = User::where('name', "=", $user->name)
                        ->select('id', 'name', 'api_token AS apiKey')
                        ->first();
        

        return $user;
    }
    public function login(Request $request)
    {   
        $inputs = $request;
        $user = User::where('name', "=", $inputs->name)
                        ->where('password', "=", $inputs->password)
                        ->select('id', 'name', 'api_token AS apiKey')
                        ->first();
        if ($user == null)
        {
            if(User::where('name', "=", $inputs->name)->first() == null)
            {
                $response = "[error501] : user not found";
            }
            else
            {
                $response = "[error501] : wrong password";
            }
            return $response;
        }

        return $user;
    }

    /**
     * Display the specified resource.
     *
     * @param  \App\User  $user
     * @return \Illuminate\Http\Response
     */
    public function show(User $user)
    {
        return $user;
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \App\User  $user
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, User $user)
    {
        $inputs = $request->except('_token', '_method', 'api_token', 'password', 'status');
        foreach($inputs as $key => $value)
        {
            $user->$key = $value;
        }
        $user->save();
        return $user;
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  \App\User  $user
     * @return \Illuminate\Http\Response
     */
    public function destroy(User $user)
    {
    //     $zoo = Zoo::where('owner_id', $user->id)->first();
    //     if ( $zoo != null)
    //     {
    //         $inventory = Inventory::where('id', $zoo->inventory_id)->first();
    //         $inventoryItems = InventoryItem::where('inventory_id', $inventory->id)->get();
    //         $inventoryItemsIds = InventoryItem::where('inventory_id', $inventory->id)->pluck('id')->all();
    //         Food::whereIn('item_id', $inventoryItemsIds)->delete();
    //         Animal::whereIn('item_id', $inventoryItemsIds)->delete();
    //         Habitat::whereIn('item_id', $inventoryItemsIds)->delete();

    //         foreach($inventoryItems as $inventoryItem)
    //         {
    //             $inventoryItem->delete();
    //         }
            
    //         $zoo->delete();
    //         $inventory->delete();

    //     }

        $user->delete();

        return response()->json();
    }
}
