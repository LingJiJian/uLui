using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_ParticleSystem_Particle : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle o;
			o=new UnityEngine.ParticleSystem.Particle();
			pushValue(l,true);
			pushValue(l,o);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_position(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.position);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_position(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.position=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_velocity(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.velocity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_velocity(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.velocity=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_lifetime(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.lifetime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_lifetime(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			float v;
			checkType(l,2,out v);
			self.lifetime=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_startLifetime(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.startLifetime);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_startLifetime(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			float v;
			checkType(l,2,out v);
			self.startLifetime=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_size(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.size);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_size(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			float v;
			checkType(l,2,out v);
			self.size=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_axisOfRotation(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.axisOfRotation);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_axisOfRotation(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			UnityEngine.Vector3 v;
			checkType(l,2,out v);
			self.axisOfRotation=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_rotation(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.rotation);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_rotation(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			float v;
			checkType(l,2,out v);
			self.rotation=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_angularVelocity(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.angularVelocity);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_angularVelocity(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			float v;
			checkType(l,2,out v);
			self.angularVelocity=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_color(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.color);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_color(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			UnityEngine.Color32 v;
			checkValueType(l,2,out v);
			self.color=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_randomSeed(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			pushValue(l,true);
			pushValue(l,self.randomSeed);
			return 2;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_randomSeed(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.Particle self;
			checkValueType(l,1,out self);
			System.UInt32 v;
			checkType(l,2,out v);
			self.randomSeed=v;
			setBack(l,self);
			pushValue(l,true);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.ParticleSystem.Particle");
		addMember(l,"position",get_position,set_position,true);
		addMember(l,"velocity",get_velocity,set_velocity,true);
		addMember(l,"lifetime",get_lifetime,set_lifetime,true);
		addMember(l,"startLifetime",get_startLifetime,set_startLifetime,true);
		addMember(l,"size",get_size,set_size,true);
		addMember(l,"axisOfRotation",get_axisOfRotation,set_axisOfRotation,true);
		addMember(l,"rotation",get_rotation,set_rotation,true);
		addMember(l,"angularVelocity",get_angularVelocity,set_angularVelocity,true);
		addMember(l,"color",get_color,set_color,true);
		addMember(l,"randomSeed",get_randomSeed,set_randomSeed,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.ParticleSystem.Particle),typeof(System.ValueType));
	}
}
