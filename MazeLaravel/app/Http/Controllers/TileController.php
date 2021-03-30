<?php

namespace App\Http\Controllers;

use App\Tile;
use Illuminate\Http\Request;

class TileController extends Controller
{
    /**
     * Display a listing of the resource.
     *
     * @return \Illuminate\Http\Response
     */
    public function index()
    {
        $tiles = Tile::paginate();
        return $tiles;
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @return \Illuminate\Http\Response
     */
    public function store(Request $request)
    {
        
        $inputs = $request->except('_token');
        $tile = new Tile();
        foreach($inputs as $key => $value) 
        {
            $tile->$key = $value;
        }
        $tile->save();

        return $tile;
    }

    /**
     * Display the specified resource.
     *
     * @param  \App\Tile  $tile
     * @return \Illuminate\Http\Response
     */
    public function show(Tile $tile)
    {
        return $tile;
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \App\Tile  $tile
     * @return \Illuminate\Http\Response
     */
    public function update(Request $request, Tile $tile)
    {
        $inputs = $request->except('_token', '_method');
        foreach($inputs as $key => $value)
        {
            $tile->$key = $value;
        }
        $tile->save();
        return $tile;
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  \App\Tile  $tile
     * @return \Illuminate\Http\Response
     */
    public function destroy(Tile $tile)
    {
        $tile->delete();
        return response()->json();
    }
}
