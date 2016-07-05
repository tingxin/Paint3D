using UnityEngine;
using System.Collections;

namespace DrawingTool
{
	public class TXQueue<T>{

		public int MaxSize{ get; private set;}

		private int addIndex = 0;
		private int getIndex = 0;

		private int loopTimes = 0;

		private T[] queue;

		public TXQueue(int maxSize){
			this.MaxSize = maxSize;
			if (this.MaxSize < 10) {
				this.MaxSize = 10;
			}
			this.queue = new T[this.MaxSize];
		}

		public void Add(T new_one){
			this.queue [this.addIndex] = new_one;

			this.addIndex++;

			if (this.addIndex > this.MaxSize) {
				this.addIndex = 0;
				this.loopTimes++;
			}
		}

		public T Get(){

			T result = this.queue [this.getIndex];
			if (this.Has) {
				this.getIndex++;
			}

			if (this.getIndex > this.MaxSize) {
				this.getIndex = 0;
				this.loopTimes--;
			}

			return result;
		}

		public bool Has{
		
			get{
				if (this.getIndex < this.addIndex + this.MaxSize * this.loopTimes) {
					return true;
				}
				return false;
			}
		}
	}

}
