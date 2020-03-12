import nltk

class NLPManager:

	def __init__(self, text=""):
		self.text = text
		
	def tag_text(self):
		tokens = nltk.word_tokenize(self.text)
		tagged = nltk.pos_tag(tokens)
		return tagged

if __name__ == '__main__':
	nlpm = NLPManager("The brown house sits in the middle of the valley.")
	print(nlpm.tag_text())
	